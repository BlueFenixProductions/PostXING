using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme RosePine { get; } = new()
    {
        Id = "rose-pine",
        Name = "Rose Pine",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#191724",            // page/editor background
            ["SurfaceVariant"] = "#1f1d2e",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#e0def4",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#ffffff",    // brightest heading text
            ["OnSurfaceVariant"] = "#908caa",   // muted/caption text
            ["OnSurfaceFaint"] = "#6e6a86",     // faint/placeholder/disabled text
            ["Primary"] = "#ebbcba",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#f0cdcc",       // slightly lighter primary
            ["PrimaryPressed"] = "#dba6a4",     // slightly darker primary
            ["OnPrimary"] = "#191724",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#9ccfd8",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#f6c177",      // an amber/orange tertiary accent
            ["FocusRing"] = "#9ccfd8",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#403d52",            // interactive border
            ["OutlineVariant"] = "#26233a",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#1f1d2e",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#26233a",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#191724", Text = "#e0def4", Comment = "#6e6a86", StringLiteral = "#f6c177",
            Number = "#ebbcba", Keyword = "#31748f", Function = "#9ccfd8", Variable = "#e0def4",
            Heading = "#c4a7e7", Punctuation = "#908caa", Caret = "#ebbcba",
            Selection = "rgba(110, 106, 134, 0.30)", Bracket = "#9ccfd8", Brace = "#f6c177",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#191724", Fg = "#e0def4", Accent = "#ebbcba",
            Link = "#c4a7e7", CodeBg = "#1f1d2e", Border = "#26233a",
        },
    };
}
