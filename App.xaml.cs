using PostXING.App.Services;
using PostXING.App.Views;

namespace PostXING.App;

public partial class App : Application
{
    public App(IThemeApplicator themes)
    {
        InitializeComponent();
        // Apply the saved theme before the first window renders. ApplyCurrent is synchronous
        // (resource writes + UserAppTheme), so it's deadlock-safe in the constructor. The applicator
        // depends on the synchronously-loaded ISettingsStore, so Current is ready here.
        themes.ApplyCurrent();
        // Re-apply on OS light/dark changes so a Sync-with-OS pair flips live (a no-op in manual
        // mode, where UserAppTheme is pinned and this doesn't fire on OS changes).
        RequestedThemeChanged += (_, _) => themes.ApplyCurrent();
    }

    protected override Window CreateWindow(IActivationState? activationState) =>
        // Launch into the brand splash; it hands the window off to AppShell when its choreography
        // finishes (see SplashPage.HandoffAsync).
        new(new SplashPage()) { Title = "PostXING 4.0" };
}
