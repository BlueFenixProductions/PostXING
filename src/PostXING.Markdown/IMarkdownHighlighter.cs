namespace PostXING.Markdown;

/// <summary>
/// Renders markdown (with optional YAML frontmatter) into spans of GitHub-Dark-Colorblind
/// themed HTML for live display inside the editor WebView. Pure function: same input ↦ same
/// output, no side effects, no IO.
/// </summary>
public interface IMarkdownHighlighter
{
    string HighlightToHtml(string markdown);
}
