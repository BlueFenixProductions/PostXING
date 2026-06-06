using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Synthwave { get; } = new()
    {
        Id = "synthwave",
        Name = "Synthwave '84",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#262335",            // page/editor background
            ["SurfaceVariant"] = "#2e2a40",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#f0eff1",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#ffffff",    // brightest heading text
            ["OnSurfaceVariant"] = "#a39fc6",   // muted/caption text
            ["OnSurfaceFaint"] = "#848bbd",     // faint/placeholder/disabled text
            ["Primary"] = "#ff7edb",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#ff96e2",       // slightly lighter primary
            ["PrimaryPressed"] = "#f25fc9",     // slightly darker primary
            ["OnPrimary"] = "#262335",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#36f9f6",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#ff8b39",      // an amber/orange tertiary accent
            ["FocusRing"] = "#36f9f6",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#495495",            // interactive border
            ["OutlineVariant"] = "#34294f",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#322d47",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#34294f",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#262335", Text = "#f0eff1", Comment = "#848bbd", StringLiteral = "#fede5d",
            Number = "#f97e72", Keyword = "#ff7edb", Function = "#36f9f6", Variable = "#ff8b39",
            Heading = "#36f9f6", Punctuation = "#f0eff1", Caret = "#ff7edb",
            Selection = "rgba(255, 126, 219, 0.30)", Bracket = "#72f1b8", Brace = "#fede5d",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#262335", Fg = "#f0eff1", Accent = "#ff7edb",
            Link = "#ff7edb", CodeBg = "#241b2f", Border = "#34294f",
        },
    };
}
