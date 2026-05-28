using PostXING.App.ViewModels;

namespace PostXING.App.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
    }
}
