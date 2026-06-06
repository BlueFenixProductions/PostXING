using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Dracula { get; } = new()
    {
        Id = "dracula",
        Name = "Dracula",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#282a36",            // page/editor background
            ["SurfaceVariant"] = "#343746",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#f8f8f2",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#ffffff",    // brightest heading text
            ["OnSurfaceVariant"] = "#a4abd4",   // muted/caption text
            ["OnSurfaceFaint"] = "#6272a4",     // faint/placeholder/disabled text
            ["Primary"] = "#bd93f9",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#cbabfa",       // slightly lighter primary
            ["PrimaryPressed"] = "#a072f0",     // slightly darker primary
            ["OnPrimary"] = "#282a36",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#8be9fd",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#ffb86c",      // an amber/orange tertiary accent
            ["FocusRing"] = "#8be9fd",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#6272a4",            // interactive border
            ["OutlineVariant"] = "#44475a",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#343746",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#44475a",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#282a36", Text = "#f8f8f2", Comment = "#6272a4", StringLiteral = "#f1fa8c",
            Number = "#bd93f9", Keyword = "#ff79c6", Function = "#50fa7b", Variable = "#ffb86c",
            Heading = "#8be9fd", Punctuation = "#f8f8f2", Caret = "#f8f8f2",
            Selection = "rgba(189, 147, 249, 0.30)", Bracket = "#8be9fd", Brace = "#ffb86c",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#282a36", Fg = "#f8f8f2", Accent = "#bd93f9",
            Link = "#bd93f9", CodeBg = "#21222c", Border = "#44475a",
        },
    };
}
