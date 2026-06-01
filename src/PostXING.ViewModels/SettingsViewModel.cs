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

    // Show/hide toggle for the PAT field so the user can verify what they pasted. Masked by default.
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaskToken))]
    private bool _showToken;

    public bool MaskToken => !ShowToken;

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
        // Clear the field only on success (don't leave the secret bound). On failure keep BOTH the
        // stored token and the field text: a transient check failure must not wipe a token the user
        // pasted, and keeping the field lets them reveal/fix it. Disconnect clears the token.
        if (IsAuthenticated) TokenInput = string.Empty;
    }

    /// <summary>Reveal/hide the pasted token so the user can verify it.</summary>
    [RelayCommand]
    private void ToggleTokenVisibility() => ShowToken = !ShowToken;

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
