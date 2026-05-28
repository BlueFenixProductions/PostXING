using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostXING.App.Services;
using PostXING.Core.Domain;
using PostXING.GitHub;
using PostXING.Markdown;

namespace PostXING.App.ViewModels;

public sealed partial class EditorViewModel : ObservableObject
{
    private readonly IFrontMatterParser _parser;
    private readonly IMarkdownRenderer _renderer;
    private readonly IGitHubGateway _gateway;
    private readonly ISettingsStore _settings;
    private readonly TimeProvider _clock;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(PublishCommand))]
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

    [ObservableProperty]
    private string _currentPath = "(new)";

    [ObservableProperty]
    private string _ghAuthStatus = "gh ?";

    [ObservableProperty]
    private bool _showTitlePrompt;

    [ObservableProperty]
    private string _promptTitle = string.Empty;

    [ObservableProperty]
    private string _promptAuthor = string.Empty;

    private bool _seeding;

    public string PreviewHtml => _renderer.RenderHtml(RawMarkdown);

    public event EventHandler? OpenPostRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? PublishConfirmationRequested;

    public EditorViewModel(
        IFrontMatterParser parser,
        IMarkdownRenderer renderer,
        IGitHubGateway gateway,
        ISettingsStore settings,
        TimeProvider clock)
    {
        _parser = parser;
        _renderer = renderer;
        _gateway = gateway;
        _settings = settings;
        _clock = clock;

        BeginNewPost();
        _ = RefreshAuthAsync();
    }

    public async Task RefreshAuthAsync()
    {
        try
        {
            var status = await _gateway.CheckAuthAsync();
            GhAuthStatus = status.IsAuthenticated
                ? (status.Username is null ? "gh ok" : $"gh: {status.Username}")
                : "gh: not auth";
        }
        catch
        {
            GhAuthStatus = "gh: error";
        }
    }

    public void LoadPost(string path, string contents)
    {
        _seeding = true;
        try
        {
            CurrentPath = path;
            RawMarkdown = contents;
            IsDirty = false;
            ShowTitlePrompt = false;
        }
        finally { _seeding = false; }
    }

    partial void OnRawMarkdownChanged(string value)
    {
        if (!_seeding) IsDirty = true;
        var parsed = _parser.Parse(value);
        FrontMatter = parsed.FrontMatter;
        var words = parsed.Body.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;
        WordCount = words;
        ReadingTimeMinutes = Math.Max(1, (int)Math.Ceiling(words / 200.0));
    }

    [RelayCommand]
    private void NewPost() => BeginNewPost();

    [RelayCommand]
    private void OpenPost() => OpenPostRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void Settings() => SettingsRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand(CanExecute = nameof(IsDirty))]
    private Task SaveAsync() => Task.CompletedTask;

    [RelayCommand(CanExecute = nameof(IsDirty))]
    private Task PublishAsync()
    {
        PublishConfirmationRequested?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ConfirmTitlePromptAsync()
    {
        var title = PromptTitle?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title)) return;

        var author = PromptAuthor?.Trim() ?? string.Empty;
        var today = DateOnly.FromDateTime(_clock.GetUtcNow().UtcDateTime);

        _seeding = true;
        try
        {
            CurrentPath = "(new)";
            RawMarkdown =
                $"---\n" +
                $"title: \"{EscapeYamlString(title)}\"\n" +
                (string.IsNullOrEmpty(author) ? string.Empty : $"author: \"{EscapeYamlString(author)}\"\n") +
                $"date: {today:yyyy-MM-dd}\n" +
                $"draft: true\n" +
                $"tags: []\n" +
                $"description: \n" +
                $"---\n\n";
            IsDirty = false;
        }
        finally { _seeding = false; }

        ShowTitlePrompt = false;

        if (!string.IsNullOrEmpty(author) && author != _settings.Current.AuthorName)
        {
            await _settings.SaveAsync(_settings.Current with { AuthorName = author });
        }
    }

    [RelayCommand]
    private void CancelTitlePrompt() => ShowTitlePrompt = false;

    private void BeginNewPost()
    {
        PromptTitle = string.Empty;
        PromptAuthor = _settings.Current.AuthorName ?? string.Empty;
        _seeding = true;
        try
        {
            CurrentPath = "(new)";
            RawMarkdown = string.Empty;
            IsDirty = false;
        }
        finally { _seeding = false; }
        ShowTitlePrompt = true;
    }

    private static string EscapeYamlString(string s) => s.Replace("\"", "\\\"");
}
