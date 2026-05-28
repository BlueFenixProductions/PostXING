using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.Core.Domain;
using PostXING.Markdown;

namespace PostXING.App.ViewModels;

public sealed partial class EditorViewModel : ObservableObject
{
    private readonly IFrontMatterParser _parser;
    private readonly IMarkdownRenderer _renderer;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private bool _isDirty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewHtml))]
    private string _rawMarkdown = string.Empty;

    [ObservableProperty]
    private int _wordCount;

    [ObservableProperty]
    private int _readingTimeMinutes = 1;

    [ObservableProperty]
    private FrontMatter _frontMatter = FrontMatter.Default;

    public string PreviewHtml => _renderer.RenderHtml(RawMarkdown);

    public event EventHandler? PublishConfirmationRequested;

    public EditorViewModel(IFrontMatterParser parser, IMarkdownRenderer renderer)
    {
        _parser = parser;
        _renderer = renderer;
    }

    partial void OnRawMarkdownChanged(string value)
    {
        IsDirty = true;
        var parsed = _parser.Parse(value);
        FrontMatter = parsed.FrontMatter;
        var words = parsed.Body.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;
        WordCount = words;
        ReadingTimeMinutes = Math.Max(1, (int)Math.Ceiling(words / 200.0));
    }

    [RelayCommand(CanExecute = nameof(IsDirty))]
    private Task SaveAsync() => Task.CompletedTask;

    [RelayCommand]
    private Task PublishAsync()
    {
        PublishConfirmationRequested?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }
}
