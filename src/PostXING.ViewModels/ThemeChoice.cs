namespace PostXING.ViewModels;

/// <summary>The user's app-theme preference. Persisted in <see cref="AppSettings"/> and mapped to
/// MAUI's <c>AppTheme</c> in the app layer (this project is off the MAUI TFM, so it can't reference
/// <c>AppTheme</c> directly). <see cref="Dark"/> is the brand default (DESIGN.md) and is the zero
/// value on purpose: a <c>settings.json</c> written before this field existed deserializes to Dark.
/// <see cref="System"/> means "follow the device theme".</summary>
public enum ThemeChoice
{
    Dark,
    Light,
    System,
}
