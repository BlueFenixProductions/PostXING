using PostXING.ViewModels;

namespace PostXING.App;

/// <summary>Maps the persisted <see cref="ThemeChoice"/> to MAUI's <see cref="AppTheme"/>.
/// <c>Unspecified</c> makes the app follow the OS theme. Used at startup (App) and when the Settings
/// picker changes (SettingsPage).</summary>
internal static class ThemeMapping
{
    public static AppTheme ToAppTheme(ThemeChoice choice) => choice switch
    {
        ThemeChoice.Light => AppTheme.Light,
        ThemeChoice.Dark => AppTheme.Dark,
        _ => AppTheme.Unspecified,   // System: follow the device theme
    };
}
