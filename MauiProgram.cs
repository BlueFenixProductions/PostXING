using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostXING.App.Services;
using PostXING.App.Views;
using PostXING.GitHub;
using PostXING.Markdown;
using PostXING.ViewModels;

namespace PostXING.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Pick up PATH changes from the registry (User + Machine scopes) before any
        // child process spawns. Without this, a winget/choco install of gh after the
        // launching shell started is invisible until the user opens a fresh shell.
        RefreshProcessPathFromRegistry();

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
#if ANDROID
        // Android can't shell out to gh; the in-process HTTP gateway talks to the GitHub REST API
        // directly, authenticating with the SecureStorage-backed token registered below.
        builder.Services.AddSingleton<IGitHubGateway>(sp => new HttpGitHubGateway(
            new HttpClient(),
            sp.GetRequiredService<IGitHubTokenStore>(),
            sp.GetRequiredService<ILogger<HttpGitHubGateway>>()));
#else
        builder.Services.AddSingleton<IGitHubGateway, GhCliGitHubGateway>();
#endif
        builder.Services.AddSingleton<IGitStatusService>(_ => new GitCliStatusService());
        builder.Services.AddSingleton<GitHubPublishService>();
        builder.Services.AddSingleton(TimeProvider.System);

        // GitHub token store: persisted via SecureStorage on Android (the in-process HTTP gateway
        // needs a stored credential there). On Windows the gh CLI owns the credential, so this is
        // registered only to satisfy DI and is never exercised.
#if ANDROID
        builder.Services.AddSingleton<IGitHubTokenStore, PostXING.App.Services.SecureStorageGitHubTokenStore>();
#else
        builder.Services.AddSingleton<IGitHubTokenStore, InMemoryGitHubTokenStore>();
#endif

#if ANDROID
        builder.Services.AddSingleton<ILocalPostStore, PostXING.App.Platforms.Android.SafLocalPostStore>();
        builder.Services.AddSingleton<IFolderPicker, PostXING.App.Platforms.Android.SafFolderPicker>();
#else
        builder.Services.AddSingleton<ILocalPostStore, FileSystemLocalPostStore>();
        builder.Services.AddSingleton<IFolderPicker, NoOpFolderPicker>();
#endif
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
        builder.Services.AddTransient<GhTerminalViewModel>();

        builder.Services.AddTransient<EditorPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<OpenPostPage>();
        builder.Services.AddTransient<GhTerminalPage>();
        builder.Services.AddTransient<AboutPage>();

        builder.Services.AddSingleton<PreviewRenderer>();
        builder.Services.AddSingleton<IPreviewStyles, PreviewStyles>();
        builder.Services.AddSingleton<IPreviewBox, PreviewBox>();
        builder.Services.AddTransient<PreviewViewModel>();
        builder.Services.AddTransient<PreviewPage>();

        return builder.Build();
    }

    private static void RefreshProcessPathFromRegistry()
    {
        // Windows-registry PATH refresh for the gh/git shell-out path. No-op elsewhere:
        // Android has no machine/user PATH registry and no CLI to find.
        if (!OperatingSystem.IsWindows()) return;

        var machinePath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine) ?? string.Empty;
        var userPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;
        var merged = string.IsNullOrEmpty(userPath) ? machinePath : $"{machinePath};{userPath}";
        if (!string.IsNullOrEmpty(merged))
            Environment.SetEnvironmentVariable("PATH", merged, EnvironmentVariableTarget.Process);
    }
}
