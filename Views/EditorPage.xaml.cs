using PostXING.App.Services;
using PostXING.App.ViewModels;

namespace PostXING.App.Views;

public partial class EditorPage : ContentPage
{
    private readonly EditorViewModel _vm;
    private readonly IPendingPostBox _box;

    public EditorPage(EditorViewModel vm, IPendingPostBox box)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _box = box;

        vm.OpenPostRequested += async (_, _) => await Shell.Current.GoToAsync("open");
        vm.SettingsRequested += async (_, _) => await Shell.Current.GoToAsync("settings");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var pending = _box.Take();
        if (pending is not null) _vm.LoadPost(pending.Path, pending.Contents);
        _ = _vm.RefreshAuthAsync();
    }
}
