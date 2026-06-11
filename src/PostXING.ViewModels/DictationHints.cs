namespace PostXING.ViewModels;

/// <summary>Which platform's built-in dictation the cue points at (#14).</summary>
public enum DictationPlatform { Windows, Android, Other }

/// <summary>
/// The transient hint the dictation mic cue shows — platform-specific, since dictation is the OS's
/// own feature (Windows Win+H, the Android keyboard mic). Pure and MAUI-free so the one piece of
/// logic is unit-testable off the MAUI TFM; the page maps the running platform and calls <see cref="For"/>.
/// </summary>
public static class DictationHints
{
    public const string Windows = "Press Win+H to dictate";
    public const string Android = "Tap the mic on your keyboard to dictate";
    public const string Default = "Use your device's voice dictation";

    public static string For(DictationPlatform platform) => platform switch
    {
        DictationPlatform.Windows => Windows,
        DictationPlatform.Android => Android,
        _ => Default,
    };
}
