using PostXING.Core.Theming;
using PostXING.ViewModels.Theming;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests.Theming;

/// <summary>The Material/accessibility gate: every curated theme must meet WCAG contrast for the
/// reading and primary-action surfaces, on all three render targets (chrome, editor, preview).
/// A theme whose hexes fail here is a real red — fix the palette, don't relax the test.</summary>
public sealed class ThemeContrastTests
{
    public static IEnumerable<object[]> AllThemes => ThemeCatalog.All.Select(t => new object[] { t });

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Body_text_meets_AA_on_background(Theme theme) =>
        Contrast.Ratio(theme.PrimaryText, theme.Background).ShouldBeGreaterThanOrEqualTo(4.5);

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Button_text_meets_AA_on_primary(Theme theme) =>
        Contrast.Ratio(theme.OnPrimary, theme.PrimaryButton).ShouldBeGreaterThanOrEqualTo(4.5);

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Kicker_accent_meets_ui_minimum_on_background(Theme theme) =>
        Contrast.Ratio(theme.Kicker, theme.Background).ShouldBeGreaterThanOrEqualTo(3.0);

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Editor_text_meets_AA_on_canvas(Theme theme) =>
        Contrast.Ratio(theme.Editor.Text, theme.Editor.Canvas).ShouldBeGreaterThanOrEqualTo(4.5);

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Preview_text_meets_AA_on_canvas(Theme theme) =>
        Contrast.Ratio(theme.Preview.Fg, theme.Preview.Canvas).ShouldBeGreaterThanOrEqualTo(4.5);
}
