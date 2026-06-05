using Microsoft.Maui.ApplicationModel;

namespace PostXING.App.Views;

// The launch splash. A staged Material-style reveal on a bold PhoenixBlue field: the amber
// halo blooms, the icon settles in with an overshoot, the wordmark rises, the version fades,
// and a sheen sweeps across the mark. Then it hands the window off to AppShell (-> OpenPostPage).
//
// This is pure view-layer choreography plus the AppInfo version string (same as AboutPage), so
// there is no unit-testable logic to extract into PostXING.ViewModels.
public partial class SplashPage : ContentPage
{
    // Material "emphasized decelerate" feel: fast out of the gate, gently settling. ~cubic-out.
    private static readonly Easing EmphasizedDecel = new(t => 1 - Math.Pow(1 - t, 3));

    // Back-out overshoot for the icon settle, so it lands with a touch of physical weight.
    private static readonly Easing Overshoot = new(t =>
    {
        const double s = 1.70158;
        t -= 1;
        return t * t * ((s + 1) * t + s) + 1;
    });

    private bool _started;
    private bool _handedOff;

    public SplashPage()
    {
        InitializeComponent();
        VersionLabel.Text = $"v{AppInfo.Current.VersionString}";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_started) return;   // OnAppearing can re-fire (e.g. window re-activation); run once.
        _started = true;

        try
        {
            await RunChoreographyAsync();
        }
        catch
        {
            // Animation can be interrupted (page detached mid-flight). Never strand the user on
            // the splash: fall through to the hand-off regardless.
        }

        await HandoffAsync();
    }

    private async Task RunChoreographyAsync()
    {
        // 1. Amber halo blooms.
        _ = AnimateGlowAsync();
        await Task.Delay(200);

        // 2. Icon settles in over the tail of the bloom.
        _ = AnimateIconAsync();
        await Task.Delay(320);

        // 3. Wordmark rises into place.
        _ = Wordmark.FadeToAsync(1, 300, EmphasizedDecel);
        _ = Wordmark.TranslateToAsync(0, 0, 300, EmphasizedDecel);
        await Task.Delay(240);

        // 4. Version fades up.
        _ = VersionLabel.FadeToAsync(1, 250, EmphasizedDecel);
        await Task.Delay(190);

        // 5. Sheen sweeps across the mark.
        Sheen.Opacity = 1;
        await Sheen.TranslateToAsync(150, 0, 650, Easing.SinInOut);
        Sheen.Opacity = 0;

        // 6. Brief hold so the finished frame reads before the hand-off.
        await Task.Delay(500);
    }

    private async Task AnimateGlowAsync()
    {
        _ = Glow.FadeToAsync(1, 500, EmphasizedDecel);
        await Glow.ScaleToAsync(1.0, 600, EmphasizedDecel);
    }

    private async Task AnimateIconAsync()
    {
        _ = IconBadge.FadeToAsync(1, 420, EmphasizedDecel);
        await IconBadge.ScaleToAsync(1.0, 520, Overshoot);
    }

    // Fade the content and swap the window root to the real app. Guarded so the normal end-of-
    // choreography call and a tap-to-skip can't both swap.
    private async Task HandoffAsync()
    {
        if (_handedOff) return;
        _handedOff = true;

        await ContentLayer.FadeToAsync(0, 300, Easing.CubicIn);

        var windows = Application.Current?.Windows;
        var window = Window ?? (windows is { Count: > 0 } ? windows[0] : null);
        if (window is not null)
            window.Page = new AppShell();
    }

    private async void OnSkipTapped(object? sender, TappedEventArgs e) => await HandoffAsync();
}
