using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using PostXING.App.Services;
using PostXING.ViewModels;
using PostXING.ViewModels.Theming;

namespace PostXING.App.Views;

internal static class BridgeLog
{
    // AppDataDirectory is cross-platform (per-user app data on Windows, app-private
    // files dir on Android), so the bridge trace works the same on both heads.
    private static readonly string LogPath = Path.Combine(FileSystem.AppDataDirectory, "bridge.log");
    private static readonly Lock Sync = new();

    static BridgeLog()
    {
        try
        {
            var dir = Path.GetDirectoryName(LogPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            File.AppendAllText(LogPath, $"\n=== {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} session start ===\n");
        }
        catch { /* swallow */ }
    }

    public static void Write(string message)
    {
        try
        {
#if ANDROID
            // run-as can't read the file on a hardened ROM (CalyxOS), so mirror to logcat:
            //   adb logcat -s PXBRIDGE
            Android.Util.Log.Info("PXBRIDGE", message);
#endif
            lock (Sync)
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} {message}\n");
            }
        }
        catch { /* swallow */ }
    }
}

public partial class EditorPage : ContentPage
{
    private readonly EditorViewModel _vm;
    private readonly IPendingPostBox _box;
    private readonly IPreviewBox _previewBox;
    private readonly IThemeApplicator _themes;

    private string _lastSyncedText = string.Empty;
    private bool _suppressOutgoingPropertyChanged;

    public EditorPage(EditorViewModel vm, IPendingPostBox box, IPreviewBox previewBox, IThemeApplicator themes)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _box = box;
        _previewBox = previewBox;
        _themes = themes;

        // Open is the Shell root and the editor is pushed on top of it, so "open" pops
        // back to the home screen rather than pushing a second copy of it.
        vm.OpenPostRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        vm.SettingsRequested += async (_, _) => await Shell.Current.GoToAsync("settings");
        vm.AboutRequested += async (_, _) => await Shell.Current.GoToAsync("about");
        vm.PreviewRequested += async (_, _) =>
        {
#if ANDROID
            // The JS->host bridge can't live-sync edits, so RawMarkdown is the last seeded text
            // unless we pull. Without this, opening Preview after typing shows stale content.
            await SyncEditorTextBeforeSaveAsync();
#endif
            _previewBox.Put(_vm.RawMarkdown);
            await Shell.Current.GoToAsync("preview");
        };
        vm.PropertyChanged += OnViewModelPropertyChanged;

#if ANDROID
        // Android's JS->host bridge can't live-sync edits, so pull the editor text on save.
        vm.SyncBeforeSaveAsync = () => SyncEditorTextBeforeSaveAsync();
#endif

        // HybridWebView serves Resources/Raw/editor (HybridRoot) and exposes a raw
        // message channel on both Windows and Android. JS -> host arrives here.
        EditorWebView.RawMessageReceived += OnRawMessageReceived;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Re-theme the editor live when the active theme changes; the current palette is seeded below.
        _themes.EditorPaletteChanged += OnEditorPaletteChanged;
        var pending = _box.Take();
        if (pending is not null) _vm.LoadPost(pending.Handle, pending.Contents);
        _ = _vm.RefreshAuthAsync();
        _ = _vm.RefreshSyncAsync(fetch: true);   // background git fetch + recompute the sync chip
        _ = SeedEditorAsync();
#if ANDROID
        StartDirtyPoll();
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _themes.EditorPaletteChanged -= OnEditorPaletteChanged;
#if ANDROID
        StopDirtyPoll();
#endif
    }

    // The page loads asynchronously and the JS->host bridge is dead on Android, so there's no
    // 'ready' signal to wait on. Fire the current text + theme on a short schedule so the seed
    // lands as soon as window.PostXING (defined by index.html) exists; the if-guard in the JS
    // makes pre-load pushes harmless no-ops. After this, OnViewModelPropertyChanged keeps the
    // editor in sync (e.g. the title-prompt seed once the user confirms).
    private async Task SeedEditorAsync()
    {
        for (var i = 0; i < 12; i++)   // ~3s of attempts to cover page load
        {
            await PushPaletteAsync(_themes.CurrentEditorPalette);
            await PushTextAsync(_vm.RawMarkdown);
            await Task.Delay(250);
        }
    }

    private async void OnRawMessageReceived(object? sender, HybridWebViewRawMessageReceivedEventArgs e)
    {
        var raw = e.Message;
        BridgeLog.Write($"RawMessageReceived raw='{raw}'");
        if (string.IsNullOrEmpty(raw)) return;

        try
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;
            if (!root.TryGetProperty("type", out var typeEl)) return;
            var type = typeEl.GetString();

            if (type == "ready")
            {
                BridgeLog.Write($"ready received → pushing {_vm.RawMarkdown.Length} chars + palette");
                await PushPaletteAsync(_themes.CurrentEditorPalette);
                await PushTextAsync(_vm.RawMarkdown);
            }
            else if (type == "change" && root.TryGetProperty("text", out var textEl))
            {
                var text = textEl.GetString() ?? string.Empty;
                if (text == _vm.RawMarkdown) return;
                _lastSyncedText = text;
                _suppressOutgoingPropertyChanged = true;
                try { _vm.RawMarkdown = text; }
                finally { _suppressOutgoingPropertyChanged = false; }
            }
        }
        catch (JsonException ex)
        {
            BridgeLog.Write($"RawMessageReceived JsonException: {ex.Message}");
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_suppressOutgoingPropertyChanged) return;
        if (e.PropertyName != nameof(EditorViewModel.RawMarkdown)) return;
        if (_vm.RawMarkdown == _lastSyncedText) return;
        await PushTextAsync(_vm.RawMarkdown);
    }

    private async void OnEditorPaletteChanged(object? sender, EditorPalette palette) =>
        await PushPaletteAsync(palette);

    private Task PushTextAsync(string text)
    {
        var t = text ?? string.Empty;
        _lastSyncedText = t;

        // Base64 so arbitrary markdown (quotes, newlines, unicode) survives MAUI's
        // EvaluateJavaScriptAsync URL-encode round-trip; index.html base64-decodes in setTextB64.
        var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(t));
        BridgeLog.Write($"PushText firing {t.Length} chars");

        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                // Fire-and-forget: MAUI's EvaluateJavaScriptAsync Task never completes on this
                // Android HybridWebView, but the JS still runs. Awaiting it would hang. The
                // if-guard makes a push before index.html has loaded a harmless no-op.
                _ = EditorWebView.EvaluateJavaScriptAsync($"if(window.PostXING){{window.PostXING.setTextB64('{b64}')}}");
            }
            catch (Exception ex) { BridgeLog.Write($"PushText threw {ex.GetType().Name}: {ex.Message}"); }
        });
        return Task.CompletedTask;
    }

    private Task PushPaletteAsync(EditorPalette palette)
    {
        // Serialize the 14 editor CSS vars to JSON, then base64 - same pipeline as PushTextAsync, so
        // the rgba()/unicode values survive the EvaluateJavaScriptAsync round-trip on both heads.
        var map = new Dictionary<string, string>(palette.ToCssVars());
        var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(map)));
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try { _ = EditorWebView.EvaluateJavaScriptAsync($"if(window.PostXING){{window.PostXING.setPaletteB64('{b64}')}}"); }
            catch (Exception ex) { BridgeLog.Write($"PushPalette threw {ex.GetType().Name}: {ex.Message}"); }
        });
        return Task.CompletedTask;
    }

    // Sync chip action sheet — commit / push / pull / refresh. The chip is Windows-only
    // (the bottom bar is hidden on Android via OnPlatform), so this never fires on Android
    // even though the handler compiles for both heads.
    private async void OnSyncChipTapped(object? sender, TappedEventArgs e)
    {
        const string commitPush = "commit & push";
        const string commitOnly = "commit";
        const string push = "push";
        const string pull = "pull";
        const string refresh = "refresh";
        var choice = await DisplayActionSheetAsync("git", "cancel", null, commitPush, commitOnly, push, pull, refresh);
        switch (choice)
        {
            case commitPush:
            {
                var msg = await PromptCommitMessageAsync();
                if (msg is null) return;
                await _vm.CommitAsync(msg);
                await _vm.PushLocalAsync();
                break;
            }
            case commitOnly:
            {
                var msg = await PromptCommitMessageAsync();
                if (msg is null) return;
                await _vm.CommitAsync(msg);
                break;
            }
            case push: await _vm.PushLocalAsync(); break;
            case pull: await _vm.PullLocalAsync(); break;
            case refresh: await _vm.RefreshSyncAsync(fetch: true); break;
        }
    }

    private async Task<string?> PromptCommitMessageAsync()
    {
        var msg = await DisplayPromptAsync("commit", "what changed?",
            accept: "commit", cancel: "cancel",
            initialValue: "WIP draft", maxLength: 240);
        return string.IsNullOrWhiteSpace(msg) ? null : msg;
    }

#if ANDROID
    // The JS->host bridge is dead on Android, so a keystroke can't push a 'dirty' signal the way
    // it does on desktop (RawMessageReceived "change"). Instead, poll the editor text on a timer
    // while the page is visible: when it diverges from RawMarkdown, SyncEditorTextBeforeSaveAsync
    // mirrors it into the ViewModel, which flips IsDirty (OnRawMarkdownChanged) and enables
    // Save/Publish — and keeps the word-count/reading-time live as a bonus.
    private IDispatcherTimer? _dirtyPollTimer;
    private bool _dirtyPollBusy;

    private void StartDirtyPoll()
    {
        if (_dirtyPollTimer is not null) return;
        _dirtyPollTimer = Dispatcher.CreateTimer();
        _dirtyPollTimer.Interval = TimeSpan.FromMilliseconds(750);
        _dirtyPollTimer.Tick += OnDirtyPollTick;
        _dirtyPollTimer.Start();
    }

    private void StopDirtyPoll()
    {
        if (_dirtyPollTimer is null) return;
        _dirtyPollTimer.Stop();
        _dirtyPollTimer.Tick -= OnDirtyPollTick;
        _dirtyPollTimer = null;
    }

    // Non-reentrant: a tick's eval can outlast the interval (it has a defensive timeout), so skip
    // a tick while the previous pull is still in flight rather than piling up overlapping evals.
    private async void OnDirtyPollTick(object? sender, EventArgs e)
    {
        if (_dirtyPollBusy) return;
        _dirtyPollBusy = true;
        try { await SyncEditorTextBeforeSaveAsync(fromPoll: true); }
        finally { _dirtyPollBusy = false; }
    }

    // Pull the editor's current text via EvaluateJavaScriptAsync (which returns at stable times,
    // unlike during page load) into the ViewModel. Echo push-back is suppressed, and it times out
    // defensively so it never blocks if eval hangs. Used both before an explicit Save/Preview and
    // by the dirty-poll timer (fromPoll); the poll path refuses to let a spurious empty read wipe a
    // non-empty doc, while an explicit save still honors an intentionally emptied editor.
    private async Task SyncEditorTextBeforeSaveAsync(bool fromPoll = false)
    {
        try
        {
            var evalTask = MainThread.InvokeOnMainThreadAsync(
                () => EditorWebView.EvaluateJavaScriptAsync("window.PostXING.getText()"));
            var done = await Task.WhenAny(evalTask, Task.Delay(2500));
            if (done != evalTask) { if (!fromPoll) BridgeLog.Write("SyncBeforeSave: getText timed out"); return; }

            var text = DecodeEvalString(evalTask.Result);
            if (text == _vm.RawMarkdown) return;
            if (fromPoll && string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(_vm.RawMarkdown)) return;

            _lastSyncedText = text;
            _suppressOutgoingPropertyChanged = true;
            try { _vm.RawMarkdown = text; }
            finally { _suppressOutgoingPropertyChanged = false; }
            if (!fromPoll) BridgeLog.Write($"SyncBeforeSave pulled {text.Length} chars");
        }
        catch (Exception ex) { BridgeLog.Write($"SyncBeforeSave threw {ex.GetType().Name}: {ex.Message}"); }
    }

    // EvaluateJavaScriptAsync returns a JS string JSON-escaped (\n, \", maybe outer-quoted);
    // decode it back to the raw markdown source.
    private static string DecodeEvalString(string? raw)
    {
        if (string.IsNullOrEmpty(raw)) return string.Empty;
        var json = raw.Length >= 2 && raw[0] == '"' && raw[^1] == '"' ? raw : "\"" + raw + "\"";
        try { return JsonSerializer.Deserialize<string>(json) ?? raw; }
        catch { return raw; }
    }
#endif
}
