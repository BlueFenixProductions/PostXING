using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme GithubLight { get; } = new()
    {
        Id = "github-light",
        Name = "GitHub Light",
        Brightness = Brightness.Light,
        GlowOpacity = 0.35,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#ffffff",            // page/editor background
            ["SurfaceVariant"] = "#f6f8fa",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#1f2328",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#010409",    // brightest heading text
            ["OnSurfaceVariant"] = "#59636e",   // muted/caption text
            ["OnSurfaceFaint"] = "#818b98",     // faint/placeholder/disabled text
            ["Primary"] = "#0969da",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#218bff",       // slightly lighter primary
            ["PrimaryPressed"] = "#0550ae",     // slightly darker primary
            ["OnPrimary"] = "#ffffff",          // text on Primary (must hit >=4.5 on Primary)
            ["KickerAccent"] = "#0a7c86",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#bc4c00",      // an amber/orange tertiary accent
            ["FocusRing"] = "#0969da",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#d0d7de",            // interactive border
            ["OutlineVariant"] = "#eaeef2",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#f3f4f6",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#eef1f4",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#ffffff", Text = "#1f2328", Comment = "#59636e", StringLiteral = "#0a3069",
            Number = "#0550ae", Keyword = "#cf222e", Function = "#8250df", Variable = "#953800",
            Heading = "#0550ae", Punctuation = "#1f2328", Caret = "#0969da",
            Selection = "rgba(9, 105, 218, 0.30)", Bracket = "#0550ae", Brace = "#8250df",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#ffffff", Fg = "#1f2328", Accent = "#0969da",
            Link = "#0969da", CodeBg = "#f6f8fa", Border = "#d0d7de",
        },
    };
}
