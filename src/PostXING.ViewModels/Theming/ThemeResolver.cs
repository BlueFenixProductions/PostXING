namespace PostXING.ViewModels.Theming;

/// <summary>Pure resolution from persisted <see cref="AppSettings"/> plus the current OS brightness
/// to the theme id to apply. With Sync-with-OS off, the explicit <see cref="AppSettings.ThemeId"/>
/// wins; with it on, the OS brightness picks the light or dark member of the chosen pair. The result
/// is always a real catalog id (a stale/removed id normalizes to the default), so callers can apply
/// it without a null check.</summary>
public static class ThemeResolver
{
    public static string Resolve(AppSettings settings, bool osIsDark)
    {
        var id = settings.SyncWithOs
            ? (osIsDark ? settings.DarkThemeId : settings.LightThemeId)
            : settings.ThemeId;
        return ThemeCatalog.Get(id).Id;
    }
}
