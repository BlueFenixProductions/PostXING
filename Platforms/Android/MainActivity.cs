using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using PostXING.App.Platforms.Android;

namespace PostXING.App;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    // AdjustResize is kept as a hint, but target SDK 35+ edge-to-edge enforcement defeats it: the
    // window no longer shrinks for the soft keyboard. So EditorPage reads the IME inset natively and
    // pushes it into the editor JS, which sizes the editor above the keyboard (GH #39). Harmless
    // where AdjustResize still works.
    WindowSoftInputMode = SoftInput.AdjustResize,
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
