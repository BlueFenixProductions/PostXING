using PostXING.ViewModels;

namespace PostXING.App.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        vm.OpenTerminalRequested += async (_, _) => await Shell.Current.GoToAsync("terminal");
        // Apply the picked theme to the live app (the VM can't touch MAUI types). Instant + sticky.
        vm.ThemeApplyRequested += (_, choice) => MainThread.BeginInvokeOnMainThread(() =>
        {
            if (Application.Current is not null)
                Application.Current.UserAppTheme = ThemeMapping.ToAppTheme(choice);
        });
    }
}
