using PostXING.Core.Theming;
using Shouldly;
using Xunit;

namespace PostXING.Core.Tests.Theming;

public sealed class ContrastTests
{
    [Fact]
    public void Ratio_of_black_on_white_is_21() =>
        Contrast.Ratio("#000000", "#FFFFFF").ShouldBe(21.0, 0.01);

    [Fact]
    public void Ratio_is_symmetric() =>
        Contrast.Ratio("#000000", "#FFFFFF").ShouldBe(Contrast.Ratio("#FFFFFF", "#000000"), 0.0001);

    [Fact]
    public void Ratio_of_identical_colors_is_1() =>
        Contrast.Ratio("#1E5BFF", "#1E5BFF").ShouldBe(1.0, 0.0001);

    [Theory]
    [InlineData("#FFF", "#FFFFFF")]
    [InlineData("#abc", "#aabbcc")]
    [InlineData("000", "#000000")] // leading # optional, shorthand expands
    public void Ratio_treats_short_and_long_hex_equivalently(string shortHex, string longHex) =>
        Contrast.Ratio(shortHex, "#808080").ShouldBe(Contrast.Ratio(longHex, "#808080"), 0.0001);

    [Theory]
    [InlineData("#777777", 4.48)] // mid-grey on white, canonical WCAG value
    [InlineData("#595959", 7.0)]
    [InlineData("#000000", 21.0)]
    public void Ratio_against_white_matches_known_values(string fg, double expected) =>
        Contrast.Ratio(fg, "#FFFFFF").ShouldBe(expected, 0.1);

    [Theory]
    [InlineData("#000000", 0.0)]
    [InlineData("#FFFFFF", 1.0)]
    public void RelativeLuminance_endpoints(string hex, double expected) =>
        Contrast.RelativeLuminance(hex).ShouldBe(expected, 0.0001);

    [Theory]
    [InlineData("#000")]
    [InlineData("#000000")]
    [InlineData("000")]
    [InlineData("#abcd")] // 4-digit (rgba hex)
    [InlineData("#aabbccdd")] // 8-digit
    public void IsValidHex_accepts_valid_forms(string hex) =>
        Contrast.IsValidHex(hex).ShouldBeTrue();

    [Theory]
    [InlineData("#GGG")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("#12")]
    [InlineData("blue")]
    [InlineData("rgba(0,0,0,1)")]
    public void IsValidHex_rejects_invalid(string s) =>
        Contrast.IsValidHex(s).ShouldBeFalse();

    [Theory]
    [InlineData("#GGG")]
    [InlineData("blue")]
    [InlineData("#12")]
    public void Ratio_throws_on_unparseable(string bad) =>
        Should.Throw<FormatException>(() => Contrast.Ratio(bad, "#FFFFFF"));
}
