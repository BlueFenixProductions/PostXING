using PostXING.App.Services;
using PostXING.App.ViewModels;

namespace PostXING.App.Views;

public partial class OpenPostPage : ContentPage
{
    public OpenPostPage(OpenPostViewModel vm, IPendingPostBox box)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.PostOpened += (_, e) => box.Put(e);
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        _ = vm.RefreshCommand.ExecuteAsync(null);
    }
}
