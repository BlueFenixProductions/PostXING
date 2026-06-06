using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme OneDark { get; } = new()
    {
        Id = "one-dark",
        Name = "One Dark",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#282c34",            // page/editor background
            ["SurfaceVariant"] = "#21252b",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#abb2bf",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#d7dae0",    // brightest heading text
            ["OnSurfaceVariant"] = "#828997",   // muted/caption text
            ["OnSurfaceFaint"] = "#5c6370",     // faint/placeholder/disabled text
            ["Primary"] = "#61afef",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#7cc0f5",       // slightly lighter primary
            ["PrimaryPressed"] = "#4a9fe0",     // slightly darker primary
            ["OnPrimary"] = "#282c34",          // text on Primary (must hit >=4.5 on Primary)
            ["KickerAccent"] = "#56b6c2",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#d19a66",      // an amber/orange tertiary accent
            ["FocusRing"] = "#61afef",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#3b4048",            // interactive border
            ["OutlineVariant"] = "#31353d",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#2f343d",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#21252b",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#282c34", Text = "#abb2bf", Comment = "#5c6370", StringLiteral = "#98c379",
            Number = "#d19a66", Keyword = "#c678dd", Function = "#61afef", Variable = "#e06c75",
            Heading = "#e5c07b", Punctuation = "#abb2bf", Caret = "#61afef",
            Selection = "rgba(97, 175, 239, 0.30)", Bracket = "#56b6c2", Brace = "#c678dd",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#282c34", Fg = "#abb2bf", Accent = "#61afef",
            Link = "#61afef", CodeBg = "#21252b", Border = "#3b4048",
        },
    };
}
