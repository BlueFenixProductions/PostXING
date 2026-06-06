using System.ComponentModel;
using PostXING.App.Services;
using PostXING.ViewModels;

namespace PostXING.App.Views;

public partial class PreviewPage : ContentPage
{
    private readonly PreviewViewModel _vm;
    private readonly IPreviewBox _box;
    private readonly IThemeApplicator _themes;

    public PreviewPage(PreviewViewModel vm, IPreviewBox box, IThemeApplicator themes)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _box = box;
        _themes = themes;

        // Pushed on top of the editor; "back" pops rather than pushing a fresh copy.
        vm.CloseRequested += async (_, _) => await Shell.Current.GoToAsync("..");
        vm.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var markdown = _box.Take();
        if (markdown is not null) _vm.SetMarkdown(markdown);
        _vm.Theme = _themes.CurrentTheme; // preview follows the active gallery theme
        await _vm.RefreshAsync();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PreviewViewModel.Html))
            PreviewWebView.Source = new HtmlWebViewSource { Html = _vm.Html };
    }
}
