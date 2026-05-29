using Microsoft.Maui.ApplicationModel;

namespace PostXING.App.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
        VersionLabel.Text = $"Version {AppInfo.Current.VersionString} (build {AppInfo.Current.BuildString})";
    }

    private async void OnLinkClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: string url } && Uri.TryCreate(url, UriKind.Absolute, out var uri))
            await Launcher.Default.OpenAsync(uri);
    }

    // About is pushed on top of whichever page opened it (Open root or the editor),
    // so ".." pops back to that page rather than pushing a fresh copy.
    private async void OnBackClicked(object? sender, EventArgs e) =>
        await Shell.Current.GoToAsync("..");
}
