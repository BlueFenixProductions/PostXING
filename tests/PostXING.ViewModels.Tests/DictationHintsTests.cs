using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

public sealed class DictationHintsTests
{
    [Theory]
    [InlineData(DictationPlatform.Windows, "Win+H")]
    [InlineData(DictationPlatform.Android, "keyboard")]
    public void For_returns_the_platform_specific_hint(DictationPlatform platform, string fragment)
    {
        DictationHints.For(platform).ShouldContain(fragment);
    }

    [Fact]
    public void For_unknown_platform_falls_back_to_a_generic_hint()
    {
        DictationHints.For(DictationPlatform.Other).ShouldBe(DictationHints.Default);
        DictationHints.For(DictationPlatform.Other).ShouldNotBeNullOrWhiteSpace();
    }
}
