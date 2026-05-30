using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.GitHub;

namespace PostXING.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsStore _store;
    private readonly IGitHubGateway _gateway;
    private readonly IFolderPicker _folderPicker;

    [ObservableProperty] private string _localFolder = string.Empty;
    [ObservableProperty] private string _owner = string.Empty;
    [ObservableProperty] private string _repo = string.Empty;
    [ObservableProperty] private string _developBranch = "develop";
    [ObservableProperty] private string _authorName = string.Empty;
    [ObservableProperty] private string _ghAuthDetail = string.Empty;
    [ObservableProperty] private bool _isAuthenticated;
    [ObservableProperty] private bool _isBusy;

    public event EventHandler? CloseRequested;
    public event EventHandler? OpenTerminalRequested;

    public SettingsViewModel(ISettingsStore store, IGitHubGateway gateway, IFolderPicker folderPicker)
    {
        _store = store;
        _gateway = gateway;
        _folderPicker = folderPicker;
        var s = store.Current;
        LocalFolder = s.LocalFolder ?? string.Empty;
        Owner = s.Owner ?? string.Empty;
        Repo = s.Repo ?? string.Empty;
        DevelopBranch = s.DevelopBranch;
        AuthorName = s.AuthorName ?? string.Empty;
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
    private void OpenTerminal() => OpenTerminalRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private async Task PickFolderAsync()
    {
        var picked = await _folderPicker.PickFolderAsync();
        if (!string.IsNullOrEmpty(picked)) LocalFolder = picked;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var s = new AppSettings(
            Owner: string.IsNullOrWhiteSpace(Owner) ? null : Owner.Trim(),
            Repo: string.IsNullOrWhiteSpace(Repo) ? null : Repo.Trim(),
            DevelopBranch: string.IsNullOrWhiteSpace(DevelopBranch) ? "develop" : DevelopBranch.Trim(),
            AuthorName: string.IsNullOrWhiteSpace(AuthorName) ? null : AuthorName.Trim(),
            LocalFolder: string.IsNullOrWhiteSpace(LocalFolder) ? null : LocalFolder.Trim());
        await _store.SaveAsync(s);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
