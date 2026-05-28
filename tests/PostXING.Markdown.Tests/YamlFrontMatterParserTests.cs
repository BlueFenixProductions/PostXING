using PostXING.Markdown;
using Shouldly;
using Xunit;

namespace PostXING.Markdown.Tests;

public sealed class YamlFrontMatterParserTests
{
    private readonly IFrontMatterParser _parser = new YamlFrontMatterParser();

    private const string Sample = """
        ---
        title: Hello World
        date: 2025-06-01
        tags:
          - dotnet
          - maui
        draft: false
        description: A first post.
        ---
        # Body content here
        """;

    [Fact]
    public void Parse_ValidDocument_ExtractsFrontMatterAndBody()
    {
        var result = _parser.Parse(Sample);
        result.FrontMatter.Title.ShouldBe("Hello World");
        result.FrontMatter.Date.ShouldBe(new DateOnly(2025, 6, 1));
        result.FrontMatter.Tags.ShouldBe(["dotnet", "maui"]);
        result.FrontMatter.Draft.ShouldBeFalse();
        result.FrontMatter.Description.ShouldBe("A first post.");
        result.Body.TrimStart().ShouldStartWith("# Body content here");
    }

    [Fact]
    public void Parse_NoFrontMatter_ReturnsEmptyTitleAndOriginalBody()
    {
        var result = _parser.Parse("# Just markdown");
        result.FrontMatter.Title.ShouldBeEmpty();
        result.Body.ShouldBe("# Just markdown");
    }
}
