using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme GithubDark { get; } = new()
    {
        Id = "github-dark",
        Name = "GitHub Dark",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#0d1117",            // page/editor background
            ["SurfaceVariant"] = "#161b22",     // raised surface / input bg (slightly off Surface)
            ["OnSurface"] = "#e6edf3",          // PRIMARY BODY TEXT (must hit >=4.5 on Surface)
            ["OnSurfaceStrong"] = "#f0f6fc",    // brightest heading text
            ["OnSurfaceVariant"] = "#9da7b3",   // muted/caption text
            ["OnSurfaceFaint"] = "#6e7681",     // faint/placeholder/disabled text
            ["Primary"] = "#1f6feb",            // the theme primary accent (button bg)
            ["PrimaryHover"] = "#2f81f7",       // slightly lighter primary
            ["PrimaryPressed"] = "#1158c7",     // slightly darker primary
            ["OnPrimary"] = "#ffffff",          // text on Primary (must hit >=4.5 on Primary)
            ["KickerAccent"] = "#39c5cf",       // a cyan/teal secondary accent (>=3 on Surface)
            ["PublishAccent"] = "#e3a008",      // an amber/orange tertiary accent
            ["FocusRing"] = "#1f6feb",          // focus ring (usually = KickerAccent or Primary)
            ["Outline"] = "#30363d",            // interactive border
            ["OutlineVariant"] = "#21262d",     // subtle divider/hairline
            ["GhostHoverSurface"] = "#171d26",  // hover fill (a touch off Surface)
            ["DisabledSurface"] = "#161b22",    // disabled control bg
            ["Scrim"] = "#8C000000",            // modal scrim (keep this value)
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#0d1117", Text = "#e6edf3", Comment = "#8b949e", StringLiteral = "#a5d6ff",
            Number = "#79c0ff", Keyword = "#ff7b72", Function = "#d2a8ff", Variable = "#ffa657",
            Heading = "#7ee787", Punctuation = "#c9d1d9", Caret = "#2f81f7",
            Selection = "rgba(56, 139, 253, 0.30)", Bracket = "#79c0ff", Brace = "#d2a8ff",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#0d1117", Fg = "#e6edf3", Accent = "#1f6feb",
            Link = "#2f81f7", CodeBg = "#161b22", Border = "#30363d",
        },
    };
}
