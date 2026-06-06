using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme CatppuccinMocha { get; } = new()
    {
        Id = "catppuccin-mocha",
        Name = "Catppuccin Mocha",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#1e1e2e",            // page/editor background
            ["SurfaceVariant"] = "#28283c",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#cdd6f4",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#ffffff",    // brightest heading text
            ["OnSurfaceVariant"] = "#9399b2",   // muted/caption text
            ["OnSurfaceFaint"] = "#7f849c",     // faint/placeholder/disabled text
            ["Primary"] = "#89b4fa",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#9cc1fb",       // slightly lighter primary
            ["PrimaryPressed"] = "#6f9cf0",     // slightly darker primary
            ["OnPrimary"] = "#1e1e2e",          // text on Primary (must hit >=4.5 on Primary: usually #fff or the dark bg)
            ["KickerAccent"] = "#94e2d5",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#fab387",      // an amber/orange tertiary accent
            ["FocusRing"] = "#94e2d5",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#585b70",            // interactive border
            ["OutlineVariant"] = "#313244",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#28283c",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#313244",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#1e1e2e", Text = "#cdd6f4", Comment = "#9399b2", StringLiteral = "#a6e3a1",
            Number = "#fab387", Keyword = "#cba6f7", Function = "#89b4fa", Variable = "#f5e0dc",
            Heading = "#94e2d5", Punctuation = "#bac2de", Caret = "#f5e0dc",
            Selection = "rgba(137, 180, 250, 0.30)", Bracket = "#94e2d5", Brace = "#f9e2af",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#1e1e2e", Fg = "#cdd6f4", Accent = "#89b4fa",
            Link = "#89b4fa", CodeBg = "#181825", Border = "#313244",
        },
    };
}
