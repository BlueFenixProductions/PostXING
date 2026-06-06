using PostXING.ViewModels;
using PostXING.ViewModels.Theming;

namespace PostXING.App.Services;

/// <summary>Applies the active theme to the live app: overwrites the semantic color tokens in
/// <c>Application.Current.Resources</c> (so every <c>{DynamicResource}</c> binding updates), sets the
/// <c>UserAppTheme</c> brightness policy, and raises <see cref="EditorPaletteChanged"/> so the editor
/// (and preview) can re-theme. Pure view-layer side effects; no I/O, so it is safe to call
/// synchronously from <c>App</c>'s constructor (the splash-deadlock contract).</summary>
public interface IThemeApplicator
{
    /// <summary>Resolve the active theme from the current settings + OS brightness and apply it.</summary>
    void ApplyCurrent();

    /// <summary>Raised with the active theme's editor palette whenever a theme is applied.</summary>
    event EventHandler<EditorPalette>? EditorPaletteChanged;
}

public sealed class ThemeApplicator : IThemeApplicator
{
    private readonly ISettingsStore _store;
    private bool _applying; // re-entrancy guard: setting UserAppTheme can re-fire RequestedThemeChanged

    public ThemeApplicator(ISettingsStore store) => _store = store;

    public event EventHandler<EditorPalette>? EditorPaletteChanged;

    public void ApplyCurrent()
    {
        var app = Application.Current;
        if (app is null || _applying) return;
        _applying = true;
        try
        {
            var settings = _store.Current;
            var theme = ThemeCatalog.Get(ThemeResolver.Resolve(settings, OsIsDark(app)));

            // Overwrite each token's value on the app-level dictionary. Re-assigning an existing key
            // is what propagates to every {DynamicResource} binding live.
            var res = app.Resources;
            foreach (var (key, hex) in theme.NativeTokens)
                res[key] = Color.FromArgb(hex);

            // Manual mode: pin UserAppTheme to the theme's brightness. Sync mode: leave it
            // Unspecified so the OS drives RequestedTheme and OS light/dark flips keep firing
            // RequestedThemeChanged (a pinned value would mask them, breaking live sync).
            app.UserAppTheme = settings.SyncWithOs
                ? AppTheme.Unspecified
                : (theme.Brightness == Brightness.Dark ? AppTheme.Dark : AppTheme.Light);

            EditorPaletteChanged?.Invoke(this, theme.Editor);
        }
        finally { _applying = false; }
    }

    // The OS theme (ignoring any UserAppTheme override), used to resolve a Sync-with-OS pair.
    private static bool OsIsDark(Application app) => app.PlatformAppTheme == AppTheme.Dark;
}
