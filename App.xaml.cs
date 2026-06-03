using PostXING.ViewModels;

namespace PostXING.App;

public partial class App : Application
{
    public App(ISettingsStore settings)
    {
        InitializeComponent();
        // Apply the saved theme before the first window renders. The store is a singleton loaded
        // synchronously at startup (MauiProgram), so Current already holds the persisted choice.
        UserAppTheme = ThemeMapping.ToAppTheme(settings.Current.Theme);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell()) { Title = "PostXING 4.0" };
    }
}
