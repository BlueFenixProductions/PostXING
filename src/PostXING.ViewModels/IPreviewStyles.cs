namespace PostXING.ViewModels;

/// <summary>Supplies the bundled github-markdown stylesheet text for the preview, per theme (colorblind dark/light).</summary>
public interface IPreviewStyles
{
    string GithubMarkdownCss(bool dark);

    /// <summary>The bundled highlight.js library text, inlined into the preview for client-side
    /// code highlighting (gh #23). Empty if unavailable — the preview still renders, just uncolored.</summary>
    string HighlightJs();

    /// <summary>The bundled highlight.js theme stylesheet for the given brightness
    /// (github-dark for dark, github for light).</summary>
    string HighlightThemeCss(bool dark);
}
