using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.Markdown;

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

    /// <summary>Set by the page from the current app theme so the github render uses the matching colorblind variant.</summary>
    public bool Dark { get; set; }

    /// <summary>Set the markdown to preview (handed off from the editor) before calling RefreshAsync.</summary>
    public void SetMarkdown(string markdown) => _markdown = markdown ?? string.Empty;

    [RelayCommand]
    public Task RefreshAsync()
    {
        IsBusy = true;
        try { Html = _renderer.Build(_markdown, _styles.GithubMarkdownCss(Dark), Dark); }
        finally { IsBusy = false; }
        return Task.CompletedTask;
    }

    [RelayCommand]
    private void Close() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
