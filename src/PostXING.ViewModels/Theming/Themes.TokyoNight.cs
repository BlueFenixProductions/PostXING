using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme TokyoNight { get; } = new()
    {
        Id = "tokyo-night",
        Name = "Tokyo Night",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#1a1b26",            // page/editor background
            ["SurfaceVariant"] = "#24283b",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#c0caf5",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#d5dbf0",    // brightest heading text
            ["OnSurfaceVariant"] = "#9aa5ce",   // muted/caption text
            ["OnSurfaceFaint"] = "#565f89",     // faint/placeholder/disabled text
            ["Primary"] = "#7aa2f7",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#8fb3f9",       // slightly lighter primary
            ["PrimaryPressed"] = "#5d86e0",     // slightly darker primary
            ["OnPrimary"] = "#1a1b26",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#7dcfff",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#ff9e64",      // an amber/orange tertiary accent
            ["FocusRing"] = "#7dcfff",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#3b4261",            // interactive border
            ["OutlineVariant"] = "#2a2e44",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#222533",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#2a2e44",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#1a1b26", Text = "#c0caf5", Comment = "#565f89", StringLiteral = "#9ece6a",
            Number = "#ff9e64", Keyword = "#bb9af7", Function = "#7aa2f7", Variable = "#c0caf5",
            Heading = "#7dcfff", Punctuation = "#89ddff", Caret = "#7dcfff",
            Selection = "rgba(122, 162, 247, 0.30)", Bracket = "#7dcfff", Brace = "#ff9e64",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#1a1b26", Fg = "#c0caf5", Accent = "#7aa2f7",
            Link = "#7aa2f7", CodeBg = "#24283b", Border = "#3b4261",
        },
    };
}
