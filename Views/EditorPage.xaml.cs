using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Web.WebView2.Core;
using PostXING.ViewModels;

namespace PostXING.App.Views;

internal static class BridgeLog
{
    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PostXING",
        "bridge.log");
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
    private const string VirtualHost = "postxing.editor";

    private readonly EditorViewModel _vm;
    private readonly IPendingPostBox _box;

    private CoreWebView2? _coreWv;
    private bool _editorReady;
    private string _lastSyncedText = string.Empty;
    private bool _suppressOutgoingPropertyChanged;

    public EditorPage(EditorViewModel vm, IPendingPostBox box)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _box = box;

        // Open is the Shell root and the editor is pushed on top of it, so "open" pops
        // back to the home screen rather than pushing a second copy of it.
        vm.OpenPostRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        vm.SettingsRequested += async (_, _) => await Shell.Current.GoToAsync("settings");
        vm.PropertyChanged += OnViewModelPropertyChanged;

        EditorWebView.HandlerChanged += OnEditorWebViewHandlerChanged;

        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnAppThemeChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var pending = _box.Take();
        if (pending is not null) _vm.LoadPost(pending.Handle, pending.Contents);
        _ = _vm.RefreshAuthAsync();
        _ = PushTextAsync(_vm.RawMarkdown);
    }

    private async void OnEditorWebViewHandlerChanged(object? sender, EventArgs e)
    {
        BridgeLog.Write("HandlerChanged fired");

        if (EditorWebView.Handler?.PlatformView is not Microsoft.UI.Xaml.Controls.WebView2 wv2)
        {
            BridgeLog.Write($"HandlerChanged: PlatformView is not WebView2 (got {EditorWebView.Handler?.PlatformView?.GetType().FullName ?? "<null>"})");
            return;
        }

        try
        {
            await wv2.EnsureCoreWebView2Async();
            var core = wv2.CoreWebView2;
            _coreWv = core;
            BridgeLog.Write("CoreWebView2 ready");

            // Map https://postxing.editor/ to the on-disk editor folder.
            var editorFolder = Path.Combine(AppContext.BaseDirectory, "editor");
            BridgeLog.Write($"Mapping virtual host {VirtualHost} → {editorFolder}");
            core.SetVirtualHostNameToFolderMapping(
                VirtualHost,
                editorFolder,
                CoreWebView2HostResourceAccessKind.Allow);

            // Inject minimal bridge: host → JS via window event; JS → host via postMessage.
            await core.AddScriptToExecuteOnDocumentCreatedAsync(@"
window.PostXINGNative = {
    postToHost: function(message) {
        if (window.chrome && window.chrome.webview && window.chrome.webview.postMessage) {
            window.chrome.webview.postMessage(message);
        }
    }
};
if (window.chrome && window.chrome.webview) {
    window.chrome.webview.addEventListener('message', function(e) {
        if (window.PostXING && typeof window.PostXING.handleHostMessage === 'function') {
            window.PostXING.handleHostMessage(e.data);
        } else {
            window.__earlyHostMessages = window.__earlyHostMessages || [];
            window.__earlyHostMessages.push(e.data);
        }
    });
}
");
            BridgeLog.Write("Bridge JS script registered");

            core.WebMessageReceived += OnWebMessageReceived;
            BridgeLog.Write("WebMessageReceived subscribed");

            // Theme defaults
            await PushThemeAsync();

            core.Navigate($"https://{VirtualHost}/index.html");
            BridgeLog.Write($"Navigate to https://{VirtualHost}/index.html");
        }
        catch (Exception ex)
        {
            BridgeLog.Write($"HandlerChanged exception {ex.GetType().Name}: {ex.Message}");
        }
    }

    private async void OnWebMessageReceived(
        CoreWebView2 sender,
        CoreWebView2WebMessageReceivedEventArgs args)
    {
        string raw;
        try { raw = args.TryGetWebMessageAsString(); }
        catch { BridgeLog.Write("WebMessageReceived: TryGetWebMessageAsString threw"); return; }

        BridgeLog.Write($"WebMessageReceived raw='{raw}'");

        try
        {
            using var doc = JsonDocument.Parse(raw ?? "{}");
            var root = doc.RootElement;
            if (!root.TryGetProperty("type", out var typeEl)) return;
            var type = typeEl.GetString();

            if (type == "ready")
            {
                _editorReady = true;
                BridgeLog.Write($"ready received → pushing {_vm.RawMarkdown.Length} chars + theme");
                // Drain any messages that arrived before the JS listener was wired.
                await PushThemeAsync();
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
            BridgeLog.Write($"WebMessageReceived JsonException: {ex.Message}");
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_suppressOutgoingPropertyChanged) return;
        if (e.PropertyName != nameof(EditorViewModel.RawMarkdown)) return;
        if (_vm.RawMarkdown == _lastSyncedText) return;
        await PushTextAsync(_vm.RawMarkdown);
    }

    private async void OnAppThemeChanged(object? sender, AppThemeChangedEventArgs e) =>
        await PushThemeAsync();

    private async Task PushTextAsync(string text)
    {
        if (!_editorReady || _coreWv is null)
        {
            BridgeLog.Write($"PushTextAsync skipped: editorReady={_editorReady}, hasCoreWv={_coreWv is not null}, text length={text?.Length ?? 0}");
            return;
        }

        var t = text ?? string.Empty;
        _lastSyncedText = t;

        var payload = JsonSerializer.Serialize(new { type = "setText", text = t });
        BridgeLog.Write($"PushTextAsync posting {t.Length} chars");

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            try
            {
                _coreWv!.PostWebMessageAsString(payload);
                BridgeLog.Write("PushTextAsync PostWebMessageAsString returned");
            }
            catch (Exception ex)
            {
                BridgeLog.Write($"PushTextAsync threw {ex.GetType().Name}: {ex.Message}");
            }
        });
    }

    private async Task PushThemeAsync()
    {
        if (_coreWv is null) return;
        var theme = Application.Current?.RequestedTheme == AppTheme.Light ? "light" : "dark";
        var payload = JsonSerializer.Serialize(new { type = "setTheme", theme });

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            try { _coreWv!.PostWebMessageAsString(payload); }
            catch (Exception ex) { BridgeLog.Write($"PushThemeAsync threw {ex.GetType().Name}: {ex.Message}"); }
        });
    }
}
