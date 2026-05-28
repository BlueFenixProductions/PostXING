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

        SeedNewPost();
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
    private void NewPost() => SeedNewPost();

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

    private void SeedNewPost()
    {
        var today = DateOnly.FromDateTime(_clock.GetUtcNow().UtcDateTime);
        _seeding = true;
        try
        {
            CurrentPath = "(new)";
            RawMarkdown = $"---\ntitle: \"\"\ndate: {today:yyyy-MM-dd}\ntags: []\ndraft: true\ndescription: \n---\n\n";
            IsDirty = false;
        }
        finally { _seeding = false; }
    }
}
