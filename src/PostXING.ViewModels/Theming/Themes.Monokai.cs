using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Monokai { get; } = new()
    {
        Id = "monokai",
        Name = "Monokai",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#272822",            // page/editor background
            ["SurfaceVariant"] = "#33342c",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#f8f8f2",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#ffffff",    // brightest heading text
            ["OnSurfaceVariant"] = "#b3b3a3",   // muted/caption text
            ["OnSurfaceFaint"] = "#75715e",     // faint/placeholder/disabled text
            ["Primary"] = "#ff5c8a",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#ff79a0",       // slightly lighter primary
            ["PrimaryPressed"] = "#f23e72",     // slightly darker primary
            ["OnPrimary"] = "#272822",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#66d9ef",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#fd971f",      // an amber/orange tertiary accent
            ["FocusRing"] = "#66d9ef",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#75715e",            // interactive border
            ["OutlineVariant"] = "#49483e",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#33342c",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#3a3b32",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#272822", Text = "#f8f8f2", Comment = "#75715e", StringLiteral = "#e6db74",
            Number = "#ae81ff", Keyword = "#f92672", Function = "#a6e22e", Variable = "#fd971f",
            Heading = "#66d9ef", Punctuation = "#f8f8f2", Caret = "#f8f8f2",
            Selection = "rgba(255, 92, 138, 0.30)", Bracket = "#66d9ef", Brace = "#fd971f",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#272822", Fg = "#f8f8f2", Accent = "#ff5c8a",
            Link = "#66d9ef", CodeBg = "#1e1f1c", Border = "#49483e",
        },
    };
}
