using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IFrontMatterParser, YamlFrontMatterParser>();
        builder.Services.AddSingleton<IMarkdownRenderer, MarkdigRenderer>();
        builder.Services.AddSingleton<IGitHubGateway, InMemoryGitHubGateway>();
        builder.Services.AddSingleton<GitHubPublishService>();

        builder.Services.AddTransient<EditorViewModel>();
        builder.Services.AddTransient<EditorPage>();

        return builder.Build();
    }
}
