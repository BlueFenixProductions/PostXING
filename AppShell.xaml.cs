using PostXING.App.Views;

namespace PostXING.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // OpenPostPage is the Shell root (the home screen); the editor is pushed on top.
        Routing.RegisterRoute("editor", typeof(EditorPage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));
        Routing.RegisterRoute("terminal", typeof(GhTerminalPage));
        Routing.RegisterRoute("about", typeof(AboutPage));
    }
}
