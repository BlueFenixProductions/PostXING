using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using PostXING.App.Platforms.Android;

namespace PostXING.App;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    // Route SAF folder-picker results to SafFolderPicker. The base class still gets every result
    // so MAUI's own activity-result clients (e.g. FilePicker) keep working.
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        if (requestCode == SafFolderPicker.OpenDocumentTreeRequestCode)
            SafFolderPicker.DeliverResult(resultCode, data);
        base.OnActivityResult(requestCode, resultCode, data);
    }
}
