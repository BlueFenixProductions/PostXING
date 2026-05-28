using PostXING.App.ViewModels;

namespace PostXING.App.Views;

public partial class EditorPage : ContentPage
{
    public EditorPage(EditorViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
