using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.App.Services;
using PostXING.GitHub;

namespace PostXING.App.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsStore _store;
    private readonly IGitHubGateway _gateway;

    [ObservableProperty] private string _owner = string.Empty;
    [ObservableProperty] private string _repo = string.Empty;
    [ObservableProperty] private string _contentRoot = "content/posts";
    [ObservableProperty] private string _developBranch = "develop";
    [ObservableProperty] private string _ghAuthDetail = string.Empty;
    [ObservableProperty] private bool _isAuthenticated;
    [ObservableProperty] private bool _isBusy;

    public event EventHandler? CloseRequested;

    public SettingsViewModel(ISettingsStore store, IGitHubGateway gateway)
    {
        _store = store;
        _gateway = gateway;
        var s = store.Current;
        Owner = s.Owner ?? string.Empty;
        Repo = s.Repo ?? string.Empty;
        ContentRoot = s.ContentRoot;
        DevelopBranch = s.DevelopBranch;
        _ = CheckAuthAsync();
    }

    [RelayCommand]
    private async Task CheckAuthAsync()
    {
        IsBusy = true;
        try
        {
            var status = await _gateway.CheckAuthAsync();
            IsAuthenticated = status.IsAuthenticated;
            GhAuthDetail = status.Detail;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var s = new AppSettings(
            Owner: string.IsNullOrWhiteSpace(Owner) ? null : Owner.Trim(),
            Repo: string.IsNullOrWhiteSpace(Repo) ? null : Repo.Trim(),
            ContentRoot: string.IsNullOrWhiteSpace(ContentRoot) ? "content/posts" : ContentRoot.Trim(),
            DevelopBranch: string.IsNullOrWhiteSpace(DevelopBranch) ? "develop" : DevelopBranch.Trim());
        await _store.SaveAsync(s);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
