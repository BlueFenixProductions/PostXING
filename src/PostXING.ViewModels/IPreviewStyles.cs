namespace PostXING.ViewModels;

/// <summary>Supplies the bundled github-markdown stylesheet text for the preview, per theme (colorblind dark/light).</summary>
public interface IPreviewStyles
{
    string GithubMarkdownCss(bool dark);
}
