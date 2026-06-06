using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

/// <summary>The curated theme gallery: the ordered list (picker display order), id lookup with a
/// graceful default fallback, and the token/var contracts every theme must satisfy. Pure and
/// static — fully unit-testable off the MAUI TFM. Theme *application* (pushing palettes to the
/// chrome/editor/preview) lives in the app layer; this is just the data + selection.</summary>
public static class ThemeCatalog
{
    public const string DefaultId = "phoenix";

    /// <summary>The semantic native color-token names every theme must define (keys into
    /// <see cref="Theme.NativeTokens"/>). These are the <c>x:Key</c>s the runtime engine overwrites.</summary>
    public static IReadOnlyList<string> RequiredNativeTokens { get; } =
    [
        "Surface", "SurfaceVariant", "OnSurface", "OnSurfaceStrong", "OnSurfaceVariant",
        "OnSurfaceFaint", "Primary", "PrimaryHover", "PrimaryPressed", "OnPrimary",
        "KickerAccent", "PublishAccent", "FocusRing", "Outline", "OutlineVariant",
        "GhostHoverSurface", "DisabledSurface", "Scrim",
    ];

    /// <summary>The 14 editor CSS-var names every theme must define (via <see cref="EditorPalette"/>).</summary>
    public static IReadOnlyList<string> RequiredEditorVars { get; } =
    [
        "--canvas", "--text", "--comment", "--string", "--number", "--keyword", "--function",
        "--variable", "--heading", "--punctuation", "--caret", "--selection", "--bracket", "--brace",
    ];

    /// <summary>All curated themes in picker order. Phoenix (brand) is first and is the default.</summary>
    public static IReadOnlyList<Theme> All { get; } = BuildAll();

    private static readonly ImmutableDictionary<string, Theme> ById =
        All.ToImmutableDictionary(t => t.Id, StringComparer.OrdinalIgnoreCase);

    public static Theme Default => ById[DefaultId];

    /// <summary>Lookup by id; an unknown or blank id falls back to <see cref="Default"/> so a
    /// <c>settings.json</c> referencing a removed/renamed theme degrades gracefully, never throws.</summary>
    public static Theme Get(string? id) =>
        id is not null && ById.TryGetValue(id, out var t) ? t : Default;

    private static IReadOnlyList<Theme> BuildAll() =>
    [
        Themes.Phoenix,
        Themes.PhoenixLight,
        Themes.GithubDark,
        Themes.GithubLight,
        Themes.TokyoNight,
        Themes.NightOwl,
        Themes.LightOwl,
        Themes.Dracula,
        Themes.Nord,
        Themes.SolarizedDark,
        Themes.SolarizedLight,
        Themes.CatppuccinMocha,
        Themes.Gruvbox,
        Themes.OneDark,
        Themes.RosePine,
        Themes.Monokai,
        Themes.Ayu,
        Themes.Synthwave,
        Themes.Hacker,
    ];
}
