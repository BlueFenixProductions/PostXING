using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme Hacker { get; } = new()
    {
        Id = "hacker",
        Name = "Hacker",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#000000",            // page/editor background
            ["SurfaceVariant"] = "#021202",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#00ff41",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#39ff14",    // brightest heading text
            ["OnSurfaceVariant"] = "#00d437",   // muted/caption text
            ["OnSurfaceFaint"] = "#00a32a",     // faint/placeholder/disabled text
            ["Primary"] = "#00ff41",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#39ff14",       // slightly lighter primary
            ["PrimaryPressed"] = "#00cc34",     // slightly darker primary
            ["OnPrimary"] = "#000000",          // text on Primary (must hit >=4.5 on Primary)
            ["KickerAccent"] = "#00ffd0",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#aaff00",      // an amber/orange tertiary accent
            ["FocusRing"] = "#00ffd0",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#005c17",            // interactive border
            ["OutlineVariant"] = "#003b00",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#021a06",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#021202",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#000000", Text = "#00ff41", Comment = "#00a32a", StringLiteral = "#39ff14",
            Number = "#00ffd0", Keyword = "#00ff41", Function = "#00ffd0", Variable = "#aaff00",
            Heading = "#39ff14", Punctuation = "#00d437", Caret = "#39ff14",
            Selection = "rgba(0, 255, 65, 0.30)", Bracket = "#00ffd0", Brace = "#aaff00",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#000000", Fg = "#00ff41", Accent = "#00ff41",
            Link = "#00ffd0", CodeBg = "#001a00", Border = "#003b00",
        },
    };
}
