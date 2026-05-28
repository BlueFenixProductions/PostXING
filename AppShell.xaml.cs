using PostXING.App.Views;

namespace PostXING.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("settings", typeof(SettingsPage));
        Routing.RegisterRoute("open", typeof(OpenPostPage));
        Routing.RegisterRoute("terminal", typeof(GhTerminalPage));
    }
}
