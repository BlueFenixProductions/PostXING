using Microsoft.Maui.Storage;
using PostXING.ViewModels;

namespace PostXING.App.Services;

/// <summary>
/// Reads the github-markdown colorblind stylesheets bundled under Resources/Raw/preview/ (a MauiAsset).
/// Uses <see cref="FileSystem.OpenAppPackageFileAsync"/> so the same call works on Windows (laid down
/// at AppContext.BaseDirectory/preview/) and on Android (inside the APK, where raw File.ReadAllText
/// would silently return empty and the preview body would render unreadably dark-on-dark). Cached.
/// </summary>
public sealed class PreviewStyles : IPreviewStyles
{
    private string? _dark;
    private string? _light;
    private string? _hljsJs;
    private string? _hljsDark;
    private string? _hljsLight;

    public string GithubMarkdownCss(bool dark) =>
        dark
            ? _dark ??= Read("github-markdown-dark-colorblind.css")
            : _light ??= Read("github-markdown-light-colorblind.css");

    public string HighlightJs() => _hljsJs ??= Read("highlight.min.js");

    public string HighlightThemeCss(bool dark) =>
        dark
            ? _hljsDark ??= Read("highlight-github-dark.css")
            : _hljsLight ??= Read("highlight-github.css");

    private static string Read(string file)
    {
        try
        {
            // Forward-slash separator — MAUI normalizes MauiAsset logical names across platforms.
            using var stream = FileSystem.OpenAppPackageFileAsync($"preview/{file}")
                .GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        catch { return string.Empty; }
    }
}
