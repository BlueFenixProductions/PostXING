using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Nord { get; } = new()
    {
        Id = "nord",
        Name = "Nord",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#2e3440",            // page/editor background
            ["SurfaceVariant"] = "#3b4252",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#d8dee9",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#eceff4",    // brightest heading text
            ["OnSurfaceVariant"] = "#aeb7c8",   // muted/caption text
            ["OnSurfaceFaint"] = "#7b88a1",     // faint/placeholder/disabled text
            ["Primary"] = "#81a1c1",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#94b1cd",       // slightly lighter primary
            ["PrimaryPressed"] = "#6d90b3",     // slightly darker primary
            ["OnPrimary"] = "#232831",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#88c0d0",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#d08770",      // an amber/orange tertiary accent
            ["FocusRing"] = "#88c0d0",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#4c566a",            // interactive border
            ["OutlineVariant"] = "#434c5e",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#3b4252",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#434c5e",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#2e3440", Text = "#d8dee9", Comment = "#616e88", StringLiteral = "#a3be8c",
            Number = "#b48ead", Keyword = "#81a1c1", Function = "#88c0d0", Variable = "#d08770",
            Heading = "#88c0d0", Punctuation = "#d8dee9", Caret = "#eceff4",
            Selection = "rgba(129, 161, 193, 0.30)", Bracket = "#88c0d0", Brace = "#ebcb8b",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#2e3440", Fg = "#d8dee9", Accent = "#81a1c1",
            Link = "#81a1c1", CodeBg = "#3b4252", Border = "#434c5e",
        },
    };
}
