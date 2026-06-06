namespace PostXING.ViewModels.Theming;

/// <summary>Whether a theme paints a dark or light canvas. Distinct from <see cref="ThemeChoice"/>
/// (the legacy Dark/Light/System preference): a theme's <see cref="Brightness"/> drives the
/// editor/preview baseline and the Sync-with-OS resolution. Dark is index 0 (the brand default).</summary>
public enum Brightness
{
    Dark,
    Light,
}
