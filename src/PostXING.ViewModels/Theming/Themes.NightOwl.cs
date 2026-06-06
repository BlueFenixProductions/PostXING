using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

public static partial class Themes
{
    public static Theme NightOwl { get; } = new()
    {
        Id = "night-owl",
        Name = "Night Owl",
        Brightness = Brightness.Dark,
        GlowOpacity = 0.6,
        NativeTokens = new Dictionary<string, string>
        {
            ["Surface"] = "#011627",
            ["SurfaceVariant"] = "#0b2942",
            ["OnSurface"] = "#d6deeb",
            ["OnSurfaceStrong"] = "#ffffff",
            ["OnSurfaceVariant"] = "#8badc1",
            ["OnSurfaceFaint"] = "#637777",
            ["Primary"] = "#82aaff",
            ["PrimaryHover"] = "#a3c0ff",
            ["PrimaryPressed"] = "#5d87e0",
            ["OnPrimary"] = "#011627",
            ["KickerAccent"] = "#7fdbca",
            ["PublishAccent"] = "#ecc48d",
            ["FocusRing"] = "#7fdbca",
            ["Outline"] = "#1d3b53",
            ["OutlineVariant"] = "#122d42",
            ["GhostHoverSurface"] = "#0b2942",
            ["DisabledSurface"] = "#0b2942",
            ["Scrim"] = "#8C000000",
        }.ToImmutableDictionary(),
        Editor = new EditorPalette
        {
            Canvas = "#011627", Text = "#d6deeb", Comment = "#637777", StringLiteral = "#ecc48d",
            Number = "#f78c6c", Keyword = "#c792ea", Function = "#82aaff", Variable = "#7fdbca",
            Heading = "#82aaff", Punctuation = "#7fdbca", Caret = "#82aaff",
            Selection = "rgba(127, 219, 202, 0.30)", Bracket = "#7fdbca", Brace = "#c792ea",
        },
        Preview = new PreviewPalette
        {
            Canvas = "#011627", Fg = "#d6deeb", Accent = "#82aaff",
            Link = "#82aaff", CodeBg = "#0b2942", Border = "#1d3b53",
        },
    };
}
