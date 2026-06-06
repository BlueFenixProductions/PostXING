using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

/// <summary>The curated theme instances — one static property per theme, split across
/// <c>Themes.*.cs</c> files by family. Phoenix (brand, dark) and Phoenix Light seed verbatim from
/// today's <c>Colors.xaml</c> + editor <c>index.html</c> so the default look is pixel-identical to
/// the pre-theme app.</summary>
public static partial class Themes
{
    public static Theme Phoenix { get; } = new()
    {
        Id = "phoenix",
        Name = "Phoenix",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#0F0F1F",
            ["SurfaceVariant"] = "#1A1A2E",
            ["OnSurface"] = "#E5E7EB",
            ["OnSurfaceStrong"] = "#FFFFFF",
            ["OnSurfaceVariant"] = "#9095A4",
            ["OnSurfaceFaint"] = "#9095A4",
            ["Primary"] = "#1E5BFF",
            ["PrimaryHover"] = "#4D7DFF",
            ["PrimaryPressed"] = "#0A2BC2",
            ["OnPrimary"] = "#FFFFFF",
            ["KickerAccent"] = "#06B6D4",
            ["PublishAccent"] = "#E08A2E",
            ["FocusRing"] = "#06B6D4",
            ["Outline"] = "#2D2D4A",
            ["OutlineVariant"] = "#2D2D4A",
            ["GhostHoverSurface"] = "#1A1A2E",
            ["DisabledSurface"] = "#2D2D4A",
            ["Scrim"] = "#8C000000",
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#0d1117", Text = "#e6edf3", Comment = "#8b949e", StringLiteral = "#a5d6ff",
            Number = "#79c0ff", Keyword = "#ff7b72", Function = "#d2a8ff", Variable = "#ffa657",
            Heading = "#79c0ff", Punctuation = "#d2a8ff", Caret = "#79c0ff",
            Selection = "rgba(56, 139, 253, 0.30)", Bracket = "#06B6D4", Brace = "#ffa657",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#0d1117", Fg = "#f0f6fc", Accent = "#1f6feb",
            Link = "#1f6feb", CodeBg = "#161b22", Border = "#30363d",
        },
    };

    public static Theme PhoenixLight { get; } = new()
    {
        Id = "phoenix-light",
        Name = "Phoenix Light",
        Brightness = Brightness.Light,
        GlowOpacity = 0.35,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#FFFFFF",
            ["SurfaceVariant"] = "#FFFFFF",
            ["OnSurface"] = "#0F0F1F",
            ["OnSurfaceStrong"] = "#0F0F1F",
            ["OnSurfaceVariant"] = "#6B7080",
            ["OnSurfaceFaint"] = "#9095A4",
            ["Primary"] = "#1E5BFF",
            ["PrimaryHover"] = "#4D7DFF",
            ["PrimaryPressed"] = "#0A2BC2",
            ["OnPrimary"] = "#FFFFFF",
            ["KickerAccent"] = "#0E7490",
            ["PublishAccent"] = "#B25C00",
            ["FocusRing"] = "#06B6D4",
            ["Outline"] = "#9095A4",
            ["OutlineVariant"] = "#E5E7EB",
            ["GhostHoverSurface"] = "#E5E7EB",
            ["DisabledSurface"] = "#2D2D4A",
            ["Scrim"] = "#8C000000",
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#ffffff", Text = "#1f2328", Comment = "#59636e", StringLiteral = "#0a3069",
            Number = "#0550ae", Keyword = "#cf222e", Function = "#8250df", Variable = "#953800",
            Heading = "#0550ae", Punctuation = "#6e7781", Caret = "#0550ae",
            Selection = "rgba(84, 174, 255, 0.30)", Bracket = "#0E7490", Brace = "#953800",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#ffffff", Fg = "#1f2328", Accent = "#0969da",
            Link = "#0969da", CodeBg = "#f6f8fa", Border = "#d0d7de",
        },
    };
}
