using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Gruvbox { get; } = new()
    {
        Id = "gruvbox",
        Name = "Gruvbox",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#282828",            // page/editor background
            ["SurfaceVariant"] = "#3c3836",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#ebdbb2",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#fbf1c7",    // brightest heading text
            ["OnSurfaceVariant"] = "#a89984",   // muted/caption text
            ["OnSurfaceFaint"] = "#7c6f64",     // faint/placeholder/disabled text
            ["Primary"] = "#fabd2f",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#fdca50",       // slightly lighter primary
            ["PrimaryPressed"] = "#e0a318",     // slightly darker primary
            ["OnPrimary"] = "#282828",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#8ec07c",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#fe8019",      // an amber/orange tertiary accent
            ["FocusRing"] = "#8ec07c",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#504945",            // interactive border
            ["OutlineVariant"] = "#3c3836",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#32302f",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#3c3836",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#282828", Text = "#ebdbb2", Comment = "#928374", StringLiteral = "#b8bb26",
            Number = "#d3869b", Keyword = "#fb4934", Function = "#fabd2f", Variable = "#83a598",
            Heading = "#8ec07c", Punctuation = "#ebdbb2", Caret = "#fbf1c7",
            Selection = "rgba(250, 189, 47, 0.30)", Bracket = "#8ec07c", Brace = "#fabd2f",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#282828", Fg = "#ebdbb2", Accent = "#fabd2f",
            Link = "#fabd2f", CodeBg = "#3c3836", Border = "#504945",
        },
    };
}
