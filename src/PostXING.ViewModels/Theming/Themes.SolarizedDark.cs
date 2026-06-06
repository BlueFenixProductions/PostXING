using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme SolarizedDark { get; } = new()
    {
        Id = "solarized-dark",
        Name = "Solarized Dark",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#002b36",            // page/editor background (base03)
            ["SurfaceVariant"] = "#073642",     // raised surface / input bg (base02)
            ["OnSurface"] = "#93a1a1",          // PRIMARY BODY TEXT (base1, 5.61 on Surface)
            ["OnSurfaceStrong"] = "#fdf4e3",    // brightest heading text (base3)
            ["OnSurfaceVariant"] = "#839496",   // muted/caption text (base0, 4.75 on Surface)
            ["OnSurfaceFaint"] = "#586e75",     // faint/placeholder/disabled text (base01)
            ["Primary"] = "#1c6aa0",            // theme primary accent (darkened blue so #fff passes)
            ["PrimaryHover"] = "#2a7cb8",       // slightly lighter primary
            ["PrimaryPressed"] = "#155680",     // slightly darker primary
            ["OnPrimary"] = "#ffffff",          // text on Primary (5.8 on Primary)
            ["KickerAccent"] = "#2aa198",       // cyan/teal secondary accent (4.75 on Surface)
            ["PublishAccent"] = "#cb4b16",      // amber/orange tertiary accent
            ["FocusRing"] = "#2aa198",          // focus ring (= KickerAccent)
            ["Outline"] = "#586e75",            // interactive border (base01)
            ["OutlineVariant"] = "#073642",     // subtle divider/hairline (base02)
            ["GhostHoverSurface"] = "#073642",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#073642",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#002b36", Text = "#93a1a1", Comment = "#586e75", StringLiteral = "#2aa198",
            Number = "#d33682", Keyword = "#859900", Function = "#268bd2", Variable = "#b58900",
            Heading = "#268bd2", Punctuation = "#839496", Caret = "#93a1a1",
            Selection = "rgba(38, 139, 210, 0.30)", Bracket = "#2aa198", Brace = "#cb4b16",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#002b36", Fg = "#93a1a1", Accent = "#268bd2",
            Link = "#268bd2", CodeBg = "#073642", Border = "#073642",
        },
    };
}
