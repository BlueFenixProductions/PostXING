using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.App.Services;
using PostXING.Core.Domain;
using PostXING.GitHub;

namespace PostXING.App.ViewModels;

public sealed partial class OpenPostViewModel : ObservableObject
{
    private readonly IGitHubGateway _gateway;
    private readonly ISettingsStore _settings;
    private readonly ILocalPostStore _local;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _statusMessage = string.Empty;

    public ObservableCollection<PostEntry> Entries { get; } = [];

    public event EventHandler<OpenedPost>? PostOpened;
    public event EventHandler? CloseRequested;
    public event EventHandler? SettingsRequested;

    public OpenPostViewModel(IGitHubGateway gateway, ISettingsStore settings, ILocalPostStore local)
    {
        _gateway = gateway;
        _settings = settings;
        _local = local;
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        var s = _settings.Current;
        IsBusy = true;
        Entries.Clear();
        try
        {
            var sources = new List<string>();
            if (s.IsLocalConfigured)
            {
                var files = _local.List(s.LocalFolder!);
                foreach (var f in files)
                    Entries.Add(new PostEntry(PostSource.LocalFile, f.FullPath, f.RelativePath, "local"));
                sources.Add($"{files.Count} local");
            }
            if (s.IsGitHubConfigured)
            {
                try
                {
                    var drafts = await _gateway.ListMarkdownFilesAsync(s.Owner!, s.Repo!, s.DevelopBranch, "drafts/");
                    var posts = await _gateway.ListMarkdownFilesAsync(s.Owner!, s.Repo!, s.DevelopBranch, "posts/");
                    foreach (var f in drafts)
                        Entries.Add(new PostEntry(PostSource.GitHub, f, f, "github draft"));
                    foreach (var f in posts)
                        Entries.Add(new PostEntry(PostSource.GitHub, f, f, "github post"));
                    sources.Add($"{drafts.Count + posts.Count} github");
                }
                catch (Exception ex)
                {
                    sources.Add($"github failed ({ex.Message})");
                }
            }

            if (!s.IsLocalConfigured && !s.IsGitHubConfigured)
                StatusMessage = "No source configured. Open Settings to add a local folder or GitHub repo.";
            else if (Entries.Count == 0)
                StatusMessage = $"No .md files found ({string.Join(", ", sources)}).";
            else
                StatusMessage = string.Join(", ", sources);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SelectAsync(PostEntry? entry)
    {
        if (entry is null) return;

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
                if (!s.IsGitHubConfigured) return;
                contents = await _gateway.GetFileContentAsync(s.Owner!, s.Repo!, s.DevelopBranch, entry.Identifier);
                handle = PostHandle.FromGitHubPath(entry.Identifier);
            }
            if (contents is null)
            {
                StatusMessage = $"Could not read {entry.DisplayName}.";
                return;
            }
            PostOpened?.Invoke(this, new OpenedPost(handle, contents));
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void OpenSettings() => SettingsRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(this, EventArgs.Empty);
}

public sealed record PostEntry(PostSource Source, string Identifier, string DisplayName, string Tag);

public sealed record OpenedPost(PostHandle Handle, string Contents);
