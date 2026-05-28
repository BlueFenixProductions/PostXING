using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.App.Services;
using PostXING.GitHub;

namespace PostXING.App.ViewModels;

public sealed partial class OpenPostViewModel : ObservableObject
{
    private readonly IGitHubGateway _gateway;
    private readonly ISettingsStore _settings;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _statusMessage = string.Empty;

    public ObservableCollection<string> Paths { get; } = [];

    public event EventHandler<OpenedPost>? PostOpened;
    public event EventHandler? CloseRequested;

    public OpenPostViewModel(IGitHubGateway gateway, ISettingsStore settings)
    {
        _gateway = gateway;
        _settings = settings;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        var s = _settings.Current;
        if (!s.IsConfigured)
        {
            StatusMessage = "No repository configured. Open Settings first.";
            return;
        }

        IsBusy = true;
        Paths.Clear();
        StatusMessage = $"Listing {s.Owner}/{s.Repo}@{s.DevelopBranch}...";
        try
        {
            var posts = await _gateway.ListMarkdownFilesAsync(s.Owner!, s.Repo!, s.DevelopBranch, "posts/");
            var drafts = await _gateway.ListMarkdownFilesAsync(s.Owner!, s.Repo!, s.DevelopBranch, "drafts/");
            foreach (var f in drafts) Paths.Add(f);
            foreach (var f in posts) Paths.Add(f);
            var total = drafts.Count + posts.Count;
            StatusMessage = total == 0 ? "No .md files in posts/ or drafts/." : $"{drafts.Count} drafts, {posts.Count} posts";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SelectAsync(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        var s = _settings.Current;
        if (!s.IsConfigured) return;

        IsBusy = true;
        try
        {
            var contents = await _gateway.GetFileContentAsync(s.Owner!, s.Repo!, s.DevelopBranch, path);
            if (contents is null)
            {
                StatusMessage = $"File {path} not found.";
                return;
            }
            PostOpened?.Invoke(this, new OpenedPost(path, contents));
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(this, EventArgs.Empty);
}

public sealed record OpenedPost(string Path, string Contents);
