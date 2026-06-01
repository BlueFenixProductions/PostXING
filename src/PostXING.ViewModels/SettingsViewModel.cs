using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.GitHub;

namespace PostXING.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsStore _store;
    private readonly IGitHubGateway _gateway;
    private readonly IFolderPicker _folderPicker;
    private readonly IGitHubTokenStore _tokens;

    [ObservableProperty] private string _localFolder = string.Empty;
    [ObservableProperty] private string _owner = string.Empty;
    [ObservableProperty] private string _repo = string.Empty;
    [ObservableProperty] private string _developBranch = "develop";
    [ObservableProperty] private string _authorName = string.Empty;
    [ObservableProperty] private string _ghAuthDetail = string.Empty;
    [ObservableProperty] private bool _isAuthenticated;
    [ObservableProperty] private bool _isBusy;

    // The PAT paste box (Android, where there's no gh to inherit a credential from). Cleared once
    // a token authenticates so the secret doesn't linger in a bound property.
    [ObservableProperty] private string _tokenInput = string.Empty;

    public event EventHandler? CloseRequested;
    public event EventHandler? OpenTerminalRequested;

    public SettingsViewModel(ISettingsStore store, IGitHubGateway gateway, IFolderPicker folderPicker, IGitHubTokenStore tokens)
    {
        _store = store;
        _gateway = gateway;
        _folderPicker = folderPicker;
        _tokens = tokens;
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

    /// <summary>Persist a pasted personal access token, then validate it via the gateway. A token
    /// that doesn't authenticate is rolled back rather than left in the store. (Android: the gh
    /// CLI isn't available there, so this is how the user supplies a credential.)</summary>
    [RelayCommand]
    private async Task PasteTokenAsync()
    {
        var token = TokenInput?.Trim();
        if (string.IsNullOrWhiteSpace(token))
        {
            GhAuthDetail = "Paste a personal access token first.";
            return;
        }

        await _tokens.SetTokenAsync(token);
        await CheckAuthAsync();            // reads the just-stored token back through the gateway
        if (IsAuthenticated)
            TokenInput = string.Empty;     // don't keep the secret in the bound field
        else
            await _tokens.ClearAsync();    // reject a token that didn't authenticate
    }

    /// <summary>Forget the stored token and re-evaluate auth state (sign out).</summary>
    [RelayCommand]
    private async Task DisconnectAsync()
    {
        await _tokens.ClearAsync();
        TokenInput = string.Empty;
        await CheckAuthAsync();
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
