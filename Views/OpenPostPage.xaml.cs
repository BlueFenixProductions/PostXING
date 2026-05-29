using PostXING.ViewModels;

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
        vm.EditorRequested += async (_, _) => await Shell.Current.GoToAsync("editor");
        vm.SettingsRequested += async (_, _) => await Shell.Current.GoToAsync("settings");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _vm.RefreshAsync();
    }

    private void OnEntrySelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is PostEntry entry)
            _ = _vm.SelectCommand.ExecuteAsync(entry);

        // Clear so the same row can be tapped again later if needed.
        if (sender is CollectionView cv)
            cv.SelectedItem = null;
    }
}
