using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.Markdown;
using PostXING.ViewModels.Theming;

namespace PostXING.ViewModels;

public sealed partial class PreviewViewModel : ObservableObject
{
    private readonly PreviewRenderer _renderer;
    private readonly IPreviewStyles _styles;

    [ObservableProperty] private string _html = string.Empty;
    [ObservableProperty] private bool _isBusy;

    private string _markdown = string.Empty;

    public event EventHandler? CloseRequested;

    public PreviewViewModel(PreviewRenderer renderer, IPreviewStyles styles)
    {
        _renderer = renderer;
        _styles = styles;
    }

    /// <summary>The active theme; its <see cref="Theme.Preview"/> palette colors the render, and its
    /// brightness picks the bundled github (colorblind) base CSS. Set by the page from the applicator.</summary>
    public Theme Theme { get; set; } = ThemeCatalog.Default;

    /// <summary>Back-compat brightness flag: the getter reflects the theme's brightness; the setter
    /// picks the brand light/dark theme. Lets the page and existing tests drive by brightness.</summary>
    public bool Dark
    {
        get => Theme.Brightness == Brightness.Dark;
        set => Theme = value ? ThemeCatalog.Get("phoenix") : ThemeCatalog.Get("phoenix-light");
    }

    /// <summary>Set the markdown to preview (handed off from the editor) before calling RefreshAsync.</summary>
    public void SetMarkdown(string markdown) => _markdown = markdown ?? string.Empty;

    [RelayCommand]
    public Task RefreshAsync()
    {
        IsBusy = true;
        try
        {
            var p = Theme.Preview;
            Html = _renderer.Build(_markdown, _styles.GithubMarkdownCss(Dark),
                p.Canvas, p.Fg, p.Accent, p.Link, p.CodeBg, p.Border);
        }
        finally { IsBusy = false; }
        return Task.CompletedTask;
    }

    [RelayCommand]
    private void Close() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
