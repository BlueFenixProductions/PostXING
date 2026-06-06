using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.Core.Domain;
using PostXING.GitHub;
using PostXING.Markdown;

namespace PostXING.ViewModels;

public sealed partial class EditorViewModel : ObservableObject
{
    private readonly IFrontMatterParser _parser;
    private readonly IGitHubGateway _gateway;
    private readonly ISettingsStore _settings;
    private readonly ILocalPostStore _local;
    private readonly TimeProvider _clock;
    private readonly IGitStatusService _gitStatus;
    private readonly GitHubPublishService _publishService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(PublishCommand))]
    private bool _isDirty;

    [ObservableProperty]
    private string _rawMarkdown = string.Empty;

    [ObservableProperty]
    private int _wordCount;

    [ObservableProperty]
    private int _readingTimeMinutes = 1;

    [ObservableProperty]
    private FrontMatter _frontMatter = FrontMatter.Default;

    [ObservableProperty]
    private string _currentPath = "(new)";

    [ObservableProperty]
    private string _ghAuthStatus = "gh ?";

    [ObservableProperty]
    private RepoSyncState _syncState = RepoSyncState.Unknown;

    [ObservableProperty]
    private string _syncStatus = "sync ?";

    [ObservableProperty]
    private string _syncDetail = "Local repo sync vs the GitHub remote.";

    [ObservableProperty]
    private bool _showTitlePrompt;

    [ObservableProperty]
    private string _promptTitle = string.Empty;

    [ObservableProperty]
    private string _promptAuthor = string.Empty;

    [ObservableProperty]
    private string _saveStatus = string.Empty;

    // The PR opened by the last Publish, awaiting an explicit Merge. Gates MergeCommand.
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(MergeCommand))]
    private int? _pendingPullRequest;

    private PostHandle _handle = PostHandle.NewDraft;
    private bool _seeding;

    public event EventHandler? OpenPostRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? AboutRequested;
    public event EventHandler? PreviewRequested;

    public EditorViewModel(
        IFrontMatterParser parser,
        IGitHubGateway gateway,
        ISettingsStore settings,
        ILocalPostStore local,
        TimeProvider clock,
        IGitStatusService gitStatus,
        GitHubPublishService publishService)
    {
        _parser = parser;
        _gateway = gateway;
        _settings = settings;
        _local = local;
        _clock = clock;
        _gitStatus = gitStatus;
        _publishService = publishService;

        BeginNewPost();
        _ = RefreshAuthAsync();
        _ = RefreshSyncAsync(fetch: false);   // instant local baseline; the page fetches on appear
    }

    /// <summary>
    /// Recomputes the git sync chip for the configured Local Posts Folder. <paramref name="fetch"/>
    /// runs a background `git fetch` first so "needs pull" reflects the live remote.
    /// </summary>
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

    /// <summary>Stage + commit everything under the Local Posts Folder. Refresh the chip after.</summary>
    public async Task CommitAsync(string message, CancellationToken ct = default)
    {
        var folder = _settings.Current.LocalFolder;
        var result = await _gitStatus.CommitAsync(folder, message, ct);
        SaveStatus = result.Success ? $"git: {result.Detail}" : $"git commit failed: {result.Detail}";
        await RefreshSyncAsync(fetch: false);
    }

    public async Task PushLocalAsync(CancellationToken ct = default)
    {
        var folder = _settings.Current.LocalFolder;
        var result = await _gitStatus.PushAsync(folder, ct);
        SaveStatus = result.Success ? $"git: {result.Detail}" : $"git push failed: {result.Detail}";
        await RefreshSyncAsync(fetch: true);
    }

    public async Task PullLocalAsync(CancellationToken ct = default)
    {
        var folder = _settings.Current.LocalFolder;
        var result = await _gitStatus.PullAsync(folder, ct);
        SaveStatus = result.Success ? $"git: {result.Detail}" : $"git pull failed: {result.Detail}";
        await RefreshSyncAsync(fetch: true);
    }

    public async Task RefreshAuthAsync()
    {
        try
        {
            var status = await _gateway.CheckAuthAsync();
            GhAuthStatus = status.IsAuthenticated
                ? (status.Username is null ? "gh ok" : $"gh: {status.Username}")
                : "gh: not auth";
        }
        catch
        {
            GhAuthStatus = "gh: error";
        }
    }

    public void LoadPost(PostHandle handle, string contents)
    {
        _seeding = true;
        try
        {
            _handle = handle;
            CurrentPath = handle.DisplayName;
            RawMarkdown = contents;
            IsDirty = false;
            ShowTitlePrompt = false;
            SaveStatus = string.Empty;
        }
        finally { _seeding = false; }
    }

    partial void OnRawMarkdownChanged(string value)
    {
        if (!_seeding) IsDirty = true;
        var parsed = _parser.Parse(value);
        FrontMatter = parsed.FrontMatter;
        var words = parsed.Body.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;
        WordCount = words;
        ReadingTimeMinutes = Math.Max(1, (int)Math.Ceiling(words / 200.0));
    }

    [RelayCommand]
    private void NewPost() => BeginNewPost();

    [RelayCommand]
    private void OpenPost() => OpenPostRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void Settings() => SettingsRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void About() => AboutRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void Preview() => PreviewRequested?.Invoke(this, EventArgs.Empty);

    /// <summary>Host hook to pull the editor's latest text into RawMarkdown before saving.
    /// Needed on Android, where the JS-&gt;host bridge can't live-sync edits; no-op if unset.</summary>
    public Func<Task>? SyncBeforeSaveAsync { get; set; }

    [RelayCommand(CanExecute = nameof(IsDirty))]
    private async Task SaveAsync()
    {
        if (SyncBeforeSaveAsync is not null) await SyncBeforeSaveAsync();
        try
        {
            switch (_handle.Source)
            {
                case PostSource.LocalFile:
                    await _local.WriteAsync(_handle.Identifier, RawMarkdown);
                    IsDirty = false;
                    SaveStatus = $"Saved {Path.GetFileName(_handle.Identifier)}";
                    break;

                case PostSource.New:
                    var folder = _settings.Current.LocalFolder;
                    if (string.IsNullOrWhiteSpace(folder))
                    {
                        SaveStatus = "Set a local folder in Settings first.";
                        return;
                    }
                    var slug = Slug.From(string.IsNullOrWhiteSpace(FrontMatter.Title) ? "untitled" : FrontMatter.Title);
                    // The store builds the path/URI (desktop: under the folder; Android SAF: under
                    // the document tree) and returns the opaque id we re-save against thereafter.
                    var id = await _local.CreateAsync(folder, $"drafts/{slug.Value}.md", RawMarkdown);
                    _handle = PostHandle.FromLocalPath(id);
                    CurrentPath = _handle.DisplayName;
                    IsDirty = false;
                    SaveStatus = $"Saved draft as {slug.Value}.md";
                    break;

                case PostSource.GitHub:
                    var ghSettings = _settings.Current;
                    if (!ghSettings.IsGitHubConfigured)
                    {
                        SaveStatus = "Set a GitHub repo in Settings first.";
                        return;
                    }
                    var ghSite = SiteConfig.For(ghSettings.Owner!, ghSettings.Repo!) with { DevelopBranch = ghSettings.DevelopBranch };
                    var ghPath = _handle.Identifier;
                    var ghFileName = ghPath[(ghPath.LastIndexOf('/') + 1)..];
                    await _publishService.SaveToBranchAsync(
                        ghSite, ghPath, RawMarkdown,
                        commitMessage: $"edit: {ghFileName}");
                    IsDirty = false;
                    SaveStatus = $"Saved to GitHub: {ghPath}";
                    break;
            }
        }
        catch (Exception ex)
        {
            SaveStatus = $"Save failed: {ex.Message}";
        }
    }

    /// <summary>Publish the current post to the configured blog repo: open a PR from a fresh
    /// post branch into the user's target branch. No auto-merge — <see cref="MergeCommand"/> is
    /// the explicit, separate step. The published content's front matter <c>draft:</c> is flipped
    /// to false. Surfaces <see cref="PublishState"/> through <see cref="SaveStatus"/>.</summary>
    [RelayCommand(CanExecute = nameof(IsDirty))]
    private async Task PublishAsync()
    {
        if (SyncBeforeSaveAsync is not null) await SyncBeforeSaveAsync();

        var s = _settings.Current;
        if (!s.IsGitHubConfigured)
        {
            SaveStatus = "Set a GitHub repo in Settings first.";
            return;
        }

        var slug = Slug.From(string.IsNullOrWhiteSpace(FrontMatter.Title) ? "untitled" : FrontMatter.Title);
        var today = DateOnly.FromDateTime(_clock.GetUtcNow().UtcDateTime);
        var published = FrontMatterEditor.WithDraft(RawMarkdown, draft: false);
        var post = new Post(slug, FrontMatter, published, today);
        var site = SiteConfig.For(s.Owner!, s.Repo!) with { DevelopBranch = s.DevelopBranch, ContentRoot = s.ContentRoot };

        SaveStatus = "Publishing...";
        try
        {
            var state = await _publishService.PublishAsync(post, site, published, autoMerge: false);
            PendingPullRequest = state.PullRequestNumber;
            SaveStatus = DescribePublishState(state);
            if (state.Kind != PublishKind.Failed) IsDirty = false;
        }
        catch (Exception ex)
        {
            SaveStatus = $"Publish failed: {ex.Message}";
        }
    }

    /// <summary>Merge the PR opened by the last <see cref="PublishCommand"/> into the target
    /// branch — the explicit "merge" step. A direct squash-merge with no CI/check polling; the
    /// blog repo's branch rules are the user's own.</summary>
    [RelayCommand(CanExecute = nameof(CanMerge))]
    private async Task MergeAsync()
    {
        var s = _settings.Current;
        if (PendingPullRequest is not int pr || !s.IsGitHubConfigured) return;

        SaveStatus = $"Merging PR #{pr}...";
        try
        {
            var sha = await _gateway.MergePullRequestAsync(s.Owner!, s.Repo!, pr, MergeStrategy.Squash);
            SaveStatus = $"Merged PR #{pr} ({sha[..Math.Min(7, sha.Length)]})";
            PendingPullRequest = null;
        }
        catch (Exception ex)
        {
            SaveStatus = $"Merge failed: {ex.Message}";
        }
    }

    private bool CanMerge() => PendingPullRequest is not null;

    private static string DescribePublishState(PublishState state) => state.Kind switch
    {
        PublishKind.Failed => $"Publish failed: {state.FailureReason ?? "unknown error"}",
        PublishKind.Merged => $"Merged ({state.MergeCommitSha?[..Math.Min(7, state.MergeCommitSha.Length)]})",
        PublishKind.PullRequestOpen when state.PullRequestNumber is int n => $"PR #{n} opened on {state.BranchName}",
        PublishKind.BranchPushed when state.BranchName is { } b => $"Branch {b} pushed",
        _ => "Publish: done",
    };

    [RelayCommand]
    private async Task ConfirmTitlePromptAsync()
    {
        var title = PromptTitle?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title)) return;

        var author = PromptAuthor?.Trim() ?? string.Empty;
        var today = DateOnly.FromDateTime(_clock.GetUtcNow().UtcDateTime);

        _seeding = true;
        try
        {
            _handle = PostHandle.NewDraft;
            CurrentPath = "(new)";
            RawMarkdown =
                $"---\n" +
                $"title: \"{EscapeYamlString(title)}\"\n" +
                (string.IsNullOrEmpty(author) ? string.Empty : $"author: \"{EscapeYamlString(author)}\"\n") +
                $"date: {today:yyyy-MM-dd}\n" +
                $"draft: true\n" +
                $"tags: []\n" +
                $"description: \n" +
                $"---\n\n" +
                $"# {title}\n\n";
            IsDirty = true;
            SaveStatus = string.Empty;
        }
        finally { _seeding = false; }

        ShowTitlePrompt = false;

        if (!string.IsNullOrEmpty(author) && author != _settings.Current.AuthorName)
        {
            await _settings.SaveAsync(_settings.Current with { AuthorName = author });
        }
    }

    [RelayCommand]
    private void CancelTitlePrompt() => ShowTitlePrompt = false;

    private void BeginNewPost()
    {
        PromptTitle = string.Empty;
        PromptAuthor = _settings.Current.AuthorName ?? string.Empty;
        _seeding = true;
        try
        {
            _handle = PostHandle.NewDraft;
            CurrentPath = "(new)";
            RawMarkdown = string.Empty;
            IsDirty = false;
            SaveStatus = string.Empty;
        }
        finally { _seeding = false; }
        ShowTitlePrompt = true;
    }

    private static string EscapeYamlString(string s) => s.Replace("\"", "\\\"");
}
