using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme SolarizedLight { get; } = new()
    {
        Id = "solarized-light",
        Name = "Solarized Light",
        Brightness = Brightness.Light,
        GlowOpacity = 0.35,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#fdf6e3",            // page/editor background (base3)
            ["SurfaceVariant"] = "#eee8d5",     // raised surface / input bg (base2)
            ["OnSurface"] = "#586e75",          // PRIMARY BODY TEXT (base01, 4.99 on Surface)
            ["OnSurfaceStrong"] = "#073642",    // brightest heading text (base02, 12.05 on Surface)
            ["OnSurfaceVariant"] = "#657b83",   // muted/caption text (base00)
            ["OnSurfaceFaint"] = "#93a1a1",     // faint/placeholder/disabled text (base1)
            ["Primary"] = "#1c6aa0",            // theme primary accent (darkened blue so #fff passes)
            ["PrimaryHover"] = "#2a7cb8",       // slightly lighter primary
            ["PrimaryPressed"] = "#155680",     // slightly darker primary
            ["OnPrimary"] = "#ffffff",          // text on Primary (5.8 on Primary)
            ["KickerAccent"] = "#1f8d84",       // cyan/teal secondary accent (darkened cyan, 3.74 on Surface)
            ["PublishAccent"] = "#cb4b16",      // amber/orange tertiary accent (orange)
            ["FocusRing"] = "#1f8d84",          // focus ring (= KickerAccent)
            ["Outline"] = "#93a1a1",            // interactive border (base1)
            ["OutlineVariant"] = "#eee8d5",     // subtle divider/hairline (base2)
            ["GhostHoverSurface"] = "#eee8d5",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#eee8d5",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#fdf6e3", Text = "#586e75", Comment = "#93a1a1", StringLiteral = "#1f8d84",
            Number = "#d33682", Keyword = "#859900", Function = "#268bd2", Variable = "#b58900",
            Heading = "#268bd2", Punctuation = "#657b83", Caret = "#586e75",
            Selection = "rgba(38, 139, 210, 0.30)", Bracket = "#1f8d84", Brace = "#cb4b16",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#fdf6e3", Fg = "#586e75", Accent = "#1c6aa0",
            Link = "#1c6aa0", CodeBg = "#eee8d5", Border = "#eee8d5",
        },
    };
}
