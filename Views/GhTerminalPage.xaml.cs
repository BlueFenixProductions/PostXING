using PostXING.App.ViewModels;

namespace PostXING.App.Views;

public partial class GhTerminalPage : ContentPage
{
    public GhTerminalPage(GhTerminalViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
    }
}
