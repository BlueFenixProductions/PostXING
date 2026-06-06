using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Ayu { get; } = new()
    {
        Id = "ayu",
        Name = "Ayu",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#0b0e14",            // page/editor background
            ["SurfaceVariant"] = "#11151c",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#bfbdb6",           // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#e6e1cf",     // brightest heading text
            ["OnSurfaceVariant"] = "#828a99",    // muted/caption text
            ["OnSurfaceFaint"] = "#565b66",      // faint/placeholder/disabled text
            ["Primary"] = "#59c2ff",             // the theme primary accent (button bg)
            ["PrimaryHover"] = "#7accff",        // slightly lighter primary
            ["PrimaryPressed"] = "#39a6e8",      // slightly darker primary
            ["OnPrimary"] = "#0b0e14",           // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#5ccfe6",        // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#ffb454",       // an amber/orange tertiary accent
            ["FocusRing"] = "#5ccfe6",           // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#1c1f26",             // interactive border
            ["OutlineVariant"] = "#161a21",      // subtle divider/hairline
            ["GhostHoverSurface"] = "#13171e",   // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#161a21",     // disabled control bg
            ["Scrim"] = "#8C000000",             // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#0b0e14", Text = "#bfbdb6", Comment = "#565b66", StringLiteral = "#aad94c",
            Number = "#ffb454", Keyword = "#ff8f40", Function = "#ffb454", Variable = "#bfbdb6",
            Heading = "#59c2ff", Punctuation = "#5ccfe6", Caret = "#ffb454",
            Selection = "rgba(89, 194, 255, 0.30)", Bracket = "#5ccfe6", Brace = "#ffb454",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#0b0e14", Fg = "#bfbdb6", Accent = "#59c2ff",
            Link = "#59c2ff", CodeBg = "#0d1017", Border = "#1c1f26",
        },
    };
}
