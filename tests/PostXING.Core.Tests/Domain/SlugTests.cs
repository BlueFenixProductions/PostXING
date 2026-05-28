using PostXING.Core.Domain;
using Shouldly;
using Xunit;

namespace PostXING.Core.Tests.Domain;

public sealed class SlugTests
{
    [Theory]
    [InlineData("Hello World 2024", "hello-world-2024")]
    [InlineData("C# is Great!", "c-is-great")]
    [InlineData("  leading spaces", "leading-spaces")]
    [InlineData("multiple   spaces", "multiple-spaces")]
    public void From_ValidTitle_ReturnsNormalizedSlug(string title, string expected)
    {
        var slug = Slug.From(title);
        slug.Value.ShouldBe(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void From_NullOrWhitespace_Throws(string title)
    {
        Should.Throw<ArgumentException>(() => Slug.From(title));
    }

    [Fact]
    public void From_OnlyPunctuation_Throws()
    {
        Should.Throw<ArgumentException>(() => Slug.From("?!.,"));
    }
}
