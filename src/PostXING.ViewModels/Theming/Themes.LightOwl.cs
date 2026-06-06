using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme LightOwl { get; } = new()
    {
        Id = "light-owl",
        Name = "Light Owl",
        Brightness = Brightness.Light,
        GlowOpacity = 0.35,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#fbfbfb",            // page/editor background
            ["SurfaceVariant"] = "#f0f0f0",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#403f53",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#1f1d33",    // brightest heading text
            ["OnSurfaceVariant"] = "#5a6f7b",   // muted/caption text
            ["OnSurfaceFaint"] = "#90a7b2",     // faint/placeholder/disabled text
            ["Primary"] = "#1772b8",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#1f81cc",       // slightly lighter primary
            ["PrimaryPressed"] = "#125e99",     // slightly darker primary
            ["OnPrimary"] = "#ffffff",          // text on Primary (must hit >=4.5 on Primary)
            ["KickerAccent"] = "#0c969b",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#daaa01",      // an amber/orange tertiary accent
            ["FocusRing"] = "#0c969b",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#d9d9d9",            // interactive border
            ["OutlineVariant"] = "#e6e6e6",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#f2f4f5",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#ededed",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#fbfbfb", Text = "#403f53", Comment = "#90a7b2", StringLiteral = "#c96765",
            Number = "#aa0982", Keyword = "#994cc3", Function = "#4876d6", Variable = "#0c969b",
            Heading = "#288ed7", Punctuation = "#0c969b", Caret = "#288ed7",
            Selection = "rgba(40, 142, 215, 0.30)", Bracket = "#0c969b", Brace = "#994cc3",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#fbfbfb", Fg = "#403f53", Accent = "#1772b8",
            Link = "#1772b8", CodeBg = "#f0f0f0", Border = "#d9d9d9",
        },
    };
}
