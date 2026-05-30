using PostXING.ViewModels;

namespace PostXING.App.Services;

/// <summary>
/// Reads the github-markdown colorblind stylesheets bundled under Resources/Raw/preview/ (a MauiAsset,
/// laid down next to the editor assets at AppContext.BaseDirectory/preview/). Cached after first read.
/// </summary>
public sealed class PreviewStyles : IPreviewStyles
{
    private string? _dark;
    private string? _light;

    public string GithubMarkdownCss(bool dark) =>
        dark
            ? _dark ??= Read("github-markdown-dark-colorblind.css")
            : _light ??= Read("github-markdown-light-colorblind.css");

    private static string Read(string file)
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "preview", file);
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }
        catch { return string.Empty; }
    }
}
