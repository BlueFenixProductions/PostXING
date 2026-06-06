using PostXING.App.Services;
using PostXING.ViewModels;

namespace PostXING.App.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm, IThemeApplicator themes)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        vm.OpenTerminalRequested += async (_, _) => await Shell.Current.GoToAsync("terminal");
        // The VM persists the new selection first, then raises this; re-apply from the now-current
        // settings so the live app (tokens + editor + the UserAppTheme/sync policy) follows the pick.
        vm.ThemeApplyRequested += (_, _) => MainThread.BeginInvokeOnMainThread(themes.ApplyCurrent);
    }
}
