using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostXING.App.Services;
using PostXING.App.ViewModels;
using PostXING.App.Views;
using PostXING.GitHub;
using PostXING.Markdown;

namespace PostXING.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Hack-Regular.ttf", "HackRegular");
                fonts.AddFont("Hack-Bold.ttf", "HackBold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IFrontMatterParser, YamlFrontMatterParser>();
        builder.Services.AddSingleton<IMarkdownRenderer, MarkdigRenderer>();
        builder.Services.AddSingleton<IGitHubGateway, GhCliGitHubGateway>();
        builder.Services.AddSingleton<GitHubPublishService>();
        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddSingleton<ISettingsStore>(_ =>
        {
            var store = new FileSystemSettingsStore();
            store.LoadAsync().GetAwaiter().GetResult();
            return store;
        });
        builder.Services.AddSingleton<IPendingPostBox, PendingPostBox>();

        builder.Services.AddTransient<EditorViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<OpenPostViewModel>();

        builder.Services.AddTransient<EditorPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<OpenPostPage>();

        return builder.Build();
    }
}
