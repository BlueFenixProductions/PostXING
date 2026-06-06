using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.GitHub;
using PostXING.ViewModels.Theming;

namespace PostXING.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsStore _store;
    private readonly IGitHubGateway _gateway;
    private readonly IFolderPicker _folderPicker;
    private readonly IGitHubTokenStore _tokens;
    // OS brightness seam, so Sync-with-OS resolution is unit-testable without MAUI. Defaults to light.
    private readonly Func<bool> _osIsDark;

    [ObservableProperty] private string _localFolder = string.Empty;
    [ObservableProperty] private string _owner = string.Empty;
    [ObservableProperty] private string _repo = string.Empty;
    [ObservableProperty] private string _developBranch = "develop";
    [ObservableProperty] private string _contentRoot = string.Empty;
    [ObservableProperty] private string _authorName = string.Empty;

    // --- theme gallery selection. Applies instantly + sticky (ApplyAndPersist), independent of Save/Cancel. ---
    [ObservableProperty] private string _selectedThemeId = ThemeCatalog.DefaultId;
    [ObservableProperty] private bool _syncWithOs;
    [ObservableProperty] private string _lightThemeId = "phoenix-light";
    [ObservableProperty] private string _darkThemeId = "phoenix";

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

    /// <summary>The full gallery in picker order, for the live-preview-card grid.</summary>
    public IReadOnlyList<Theme> Themes { get; } = ThemeCatalog.All;

    /// <summary>Light-only / dark-only projections for the Sync-with-OS pair pickers.</summary>
    public IReadOnlyList<Theme> LightThemes { get; } =
        ThemeCatalog.All.Where(t => t.Brightness == Brightness.Light).ToList();

    public IReadOnlyList<Theme> DarkThemes { get; } =
        ThemeCatalog.All.Where(t => t.Brightness == Brightness.Dark).ToList();

    public event EventHandler? CloseRequested;
    public event EventHandler? OpenTerminalRequested;

    /// <summary>Raised with the RESOLVED theme id whenever the selection changes, so the view can
    /// apply it to the live app (the VM can't touch MAUI types directly).</summary>
    public event EventHandler<string>? ThemeApplyRequested;

    public SettingsViewModel(
        ISettingsStore store,
        IGitHubGateway gateway,
        IFolderPicker folderPicker,
        IGitHubTokenStore tokens,
        Func<bool>? osIsDark = null)
    {
        _store = store;
        _gateway = gateway;
        _folderPicker = folderPicker;
        _tokens = tokens;
        _osIsDark = osIsDark ?? (() => false);
        var s = store.Current;
        LocalFolder = s.LocalFolder ?? string.Empty;
        Owner = s.Owner ?? string.Empty;
        Repo = s.Repo ?? string.Empty;
        DevelopBranch = s.DevelopBranch;
        ContentRoot = s.ContentRoot ?? string.Empty;
        AuthorName = s.AuthorName ?? string.Empty;
        // Seed the backing fields (not the properties) so the partial OnXxxChanged hooks don't fire a
        // redundant apply/persist during construction (matches the prior _theme = s.Theme pattern).
        _selectedThemeId = s.ThemeId;
        _syncWithOs = s.SyncWithOs;
        _lightThemeId = s.LightThemeId;
        _darkThemeId = s.DarkThemeId;
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

    /// <summary>Pick a theme by id (tapped from the live-preview-card grid).</summary>
    [RelayCommand]
    private void SelectTheme(string id) => SelectedThemeId = id;

    partial void OnSelectedThemeIdChanged(string value) => ApplyAndPersist();
    partial void OnSyncWithOsChanged(bool value) => ApplyAndPersist();
    partial void OnLightThemeIdChanged(string value) => ApplyAndPersist();
    partial void OnDarkThemeIdChanged(string value) => ApplyAndPersist();

    // Theme applies instantly and sticks, like any OS theme toggle: resolve the active theme id from
    // the current selection + OS brightness, raise it for the view to apply, and persist immediately
    // off the *stored* settings (not the in-progress fields) so flipping a theme never commits a
    // half-typed repo/owner edit.
    private void ApplyAndPersist()
    {
        var next = _store.Current with
        {
            ThemeId = SelectedThemeId,
            SyncWithOs = SyncWithOs,
            LightThemeId = LightThemeId,
            DarkThemeId = DarkThemeId,
            ThemeMigrated = true,
        };
        // Persist first (SaveAsync sets the store's Current synchronously, before its first await),
        // then notify - so the view's re-apply reads the just-saved settings.
        _ = _store.SaveAsync(next);
        ThemeApplyRequested?.Invoke(this, ThemeResolver.Resolve(next, _osIsDark()));
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var s = new AppSettings(
            Owner: string.IsNullOrWhiteSpace(Owner) ? null : Owner.Trim(),
            Repo: string.IsNullOrWhiteSpace(Repo) ? null : Repo.Trim(),
            DevelopBranch: string.IsNullOrWhiteSpace(DevelopBranch) ? "develop" : DevelopBranch.Trim(),
            AuthorName: string.IsNullOrWhiteSpace(AuthorName) ? null : AuthorName.Trim(),
            LocalFolder: string.IsNullOrWhiteSpace(LocalFolder) ? null : LocalFolder.Trim(),
            ContentRoot: string.IsNullOrWhiteSpace(ContentRoot) ? null : ContentRoot.Trim(),
            Theme: _store.Current.Theme, // preserve the legacy field; behavior reads the gallery fields
            ThemeId: SelectedThemeId,
            SyncWithOs: SyncWithOs,
            LightThemeId: LightThemeId,
            DarkThemeId: DarkThemeId,
            ThemeMigrated: true);
        await _store.SaveAsync(s);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
