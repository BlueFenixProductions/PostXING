using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.Core.Domain;
using PostXING.GitHub;

namespace PostXING.ViewModels;

public sealed partial class OpenPostViewModel : ObservableObject
{
    private readonly IGitHubGateway _gateway;
    private readonly ISettingsStore _settings;
    private readonly ILocalPostStore _local;
    private readonly IGitStatusService _gitStatus;

    // Latched once we initiate navigation away from this page (open a post, "new", or
    // "settings") and cleared when the page re-appears (RefreshAsync runs from OnAppearing).
    // Guarantees a single navigation per visit, so a fast double-tap can't push two pages.
    private bool _navigated;

    [ObservableProperty] private bool _isBusy;

    // Two-way bound to the OpenPostPage RefreshView: the pull gesture sets this true and fires
    // RefreshCommand; RefreshAsync clears it when the GitHub-syncing reload finishes (gh #49).
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private string _statusMessage = string.Empty;

    [ObservableProperty] private RepoSyncState _syncState = RepoSyncState.Unknown;
    [ObservableProperty] private string _syncStatus = "sync ?";
    [ObservableProperty] private string _syncDetail = "Local repo sync vs the GitHub remote.";

    public ObservableCollection<PostEntry> Entries { get; } = [];

    public event EventHandler<OpenedPost>? PostOpened;
    public event EventHandler? EditorRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? AboutRequested;

    public OpenPostViewModel(IGitHubGateway gateway, ISettingsStore settings, ILocalPostStore local, IGitStatusService gitStatus)
    {
        _gateway = gateway;
        _settings = settings;
        _local = local;
        _gitStatus = gitStatus;
    }

    /// <summary>Recomputes the git sync chip for the Local Posts Folder (background fetch when asked).</summary>
    public async Task RefreshSyncAsync(bool fetch)
    {
        try
        {
            var status = await _gitStatus.GetStatusAsync(_settings.Current.LocalFolder, fetch);
            SyncState = status.State;
            SyncStatus = SyncChip.Label(status);
            SyncDetail = string.IsNullOrEmpty(status.Detail) ? SyncStatus : status.Detail;
        }
        catch
        {
            SyncState = RepoSyncState.Unknown;
            SyncStatus = "sync error";
            SyncDetail = "Could not read git status.";
        }
    }

    [RelayCommand]
    private Task RefreshSync() => RefreshSyncAsync(fetch: true);

    public async Task CommitAsync(string message, CancellationToken ct = default)
    {
        var folder = _settings.Current.LocalFolder;
        var result = await _gitStatus.CommitAsync(folder, message, ct);
        StatusMessage = result.Success ? $"git: {result.Detail}" : $"git commit failed: {result.Detail}";
        await RefreshSyncAsync(fetch: false);
    }

    public async Task PushLocalAsync(CancellationToken ct = default)
    {
        var folder = _settings.Current.LocalFolder;
        var result = await _gitStatus.PushAsync(folder, ct);
        StatusMessage = result.Success ? $"git: {result.Detail}" : $"git push failed: {result.Detail}";
        await RefreshSyncAsync(fetch: true);
    }

    public async Task PullLocalAsync(CancellationToken ct = default)
    {
        var folder = _settings.Current.LocalFolder;
        var result = await _gitStatus.PullAsync(folder, ct);
        StatusMessage = result.Success ? $"git: {result.Detail}" : $"git pull failed: {result.Detail}";
        // A successful pull may have brought new posts in — re-list as well as re-chip.
        if (result.Success) await RefreshAsync();
        else await RefreshSyncAsync(fetch: true);
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        // The page is (re)appearing — re-arm navigation for this visit.
        _navigated = false;
        _ = RefreshSyncAsync(fetch: true);   // refresh the sync chip alongside the post list
        var s = _settings.Current;
        IsBusy = true;
        Entries.Clear();
        try
        {
            var sources = new List<string>();
            // (sort key, entry). Local files carry a real last-write time; GitHub paths have no
            // timestamp from `gh`, so published posts sort by the yyyy-MM-dd date in their filename
            // and undated drafts sink to the bottom. Everything is presented newest-first.
            var found = new List<(DateTimeOffset Key, PostEntry Entry)>();
            if (s.IsLocalConfigured)
            {
                var files = _local.List(s.LocalFolder!);
                foreach (var f in files)
                    found.Add((f.LastWriteTimeUtc, new PostEntry(PostSource.LocalFile, f.Id, f.RelativePath, "local")));
                sources.Add($"{files.Count} local");
            }
            if (s.IsGitHubConfigured)
            {
                try
                {
                    var drafts = await _gateway.ListMarkdownFilesAsync(s.Owner!, s.Repo!, s.DevelopBranch, s.DraftsPrefix);
                    var posts = await _gateway.ListMarkdownFilesAsync(s.Owner!, s.Repo!, s.DevelopBranch, s.PostsPrefix);
                    foreach (var f in drafts)
                        found.Add((FileNameDate(f), new PostEntry(PostSource.GitHub, f, f, "github draft")));
                    foreach (var f in posts)
                        found.Add((FileNameDate(f), new PostEntry(PostSource.GitHub, f, f, "github post")));
                    sources.Add($"{drafts.Count + posts.Count} github");
                }
                catch (Exception ex)
                {
                    sources.Add($"github failed ({ex.Message})");
                }
            }

            foreach (var (_, entry) in found
                .OrderByDescending(x => x.Key)
                .ThenByDescending(x => x.Entry.DisplayName, StringComparer.Ordinal))
                Entries.Add(entry);

            if (!s.IsLocalConfigured && !s.IsGitHubConfigured)
                StatusMessage = "No source configured. Open Settings to add a local folder or GitHub repo.";
            else if (Entries.Count == 0)
                StatusMessage = $"No .md files found ({string.Join(", ", sources)}).";
            else
                StatusMessage = string.Join(", ", sources);
        }
        finally { IsBusy = false; IsRefreshing = false; }
    }

    // Published posts are named "posts/{yyyy-MM-dd}-{slug}.md"; the leading date is the only
    // ordering signal available for a GitHub path. Undated paths (drafts) get MinValue and sink.
    private static DateTimeOffset FileNameDate(string path)
    {
        var name = path[(path.LastIndexOf('/') + 1)..];
        return name.Length >= 10 && DateTimeOffset.TryParseExact(
            name[..10], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var d)
            ? d
            : DateTimeOffset.MinValue;
    }

    [RelayCommand]
    private async Task SelectAsync(PostEntry? entry)
    {
        if (entry is null || _navigated) return;
        _navigated = true;

        IsBusy = true;
        try
        {
            string? contents;
            PostHandle handle;
            if (entry.Source == PostSource.LocalFile)
            {
                contents = await _local.ReadAsync(entry.Identifier);
                handle = PostHandle.FromLocalPath(entry.Identifier);
            }
            else
            {
                var s = _settings.Current;
                if (!s.IsGitHubConfigured) { _navigated = false; return; }
                contents = await _gateway.GetFileContentAsync(s.Owner!, s.Repo!, s.DevelopBranch, entry.Identifier);
                handle = PostHandle.FromGitHubPath(entry.Identifier);
            }
            if (contents is null)
            {
                StatusMessage = $"Could not read {entry.DisplayName}.";
                _navigated = false;
                return;
            }
            PostOpened?.Invoke(this, new OpenedPost(handle, contents));
            EditorRequested?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
            _navigated = false;
        }
        finally { IsBusy = false; }
    }

    // Delete the selected post. Confirmation lives in the page (the VM can't show dialogs); by the
    // time we're here the user has confirmed. Local files delete directly; GitHub files (draft or
    // post) delete via a single commit to the integration branch, which needs the current blob sha.
    [RelayCommand]
    private async Task DeleteAsync(PostEntry? entry)
    {
        if (entry is null) return;
        IsBusy = true;
        try
        {
            if (entry.Source == PostSource.LocalFile)
            {
                await _local.DeleteAsync(entry.Identifier);
            }
            else
            {
                var s = _settings.Current;
                if (!s.IsGitHubConfigured) { StatusMessage = "No GitHub repo configured."; return; }
                var sha = await _gateway.GetFileShaAsync(s.Owner!, s.Repo!, s.DevelopBranch, entry.Identifier);
                // A null sha means it's already gone on the remote — drop the stale row, don't commit.
                if (sha is not null)
                    await _gateway.DeleteFileAsync(s.Owner!, s.Repo!, s.DevelopBranch, entry.Identifier,
                        $"Delete {entry.DisplayName}", sha);
            }
            Entries.Remove(entry);
            StatusMessage = $"Deleted {entry.DisplayName}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Delete failed: {ex.Message}";
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void OpenSettings()
    {
        if (_navigated) return;
        _navigated = true;
        SettingsRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void About()
    {
        if (_navigated) return;
        _navigated = true;
        AboutRequested?.Invoke(this, EventArgs.Empty);
    }

    // The home screen's "new" affordance: open a blank editor (empty pending box →
    // EditorViewModel's title-prompt overlay seeds the new post).
    [RelayCommand]
    private void NewPost()
    {
        if (_navigated) return;
        _navigated = true;
        EditorRequested?.Invoke(this, EventArgs.Empty);
    }
}

public sealed record PostEntry(PostSource Source, string Identifier, string DisplayName, string Tag);

public sealed record OpenedPost(PostHandle Handle, string Contents);
