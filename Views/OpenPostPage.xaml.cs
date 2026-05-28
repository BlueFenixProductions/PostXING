using PostXING.App.Services;
using PostXING.App.ViewModels;

namespace PostXING.App.Views;

public partial class OpenPostPage : ContentPage
{
    private readonly OpenPostViewModel _vm;
    private readonly IPendingPostBox _box;

    public OpenPostPage(OpenPostViewModel vm, IPendingPostBox box)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _box = box;
        vm.PostOpened += (_, e) => box.Put(e);
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        vm.SettingsRequested += async (_, _) => await Shell.Current.GoToAsync("settings");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _vm.RefreshAsync();
    }
}
