using System.Text.RegularExpressions;
using PostXING.Core.Domain;
using PostXING.Markdown;
using Shouldly;
using Xunit;

namespace PostXING.Markdown.Tests;

public sealed class YamlFrontMatterParserTests
{
    private readonly YamlFrontMatterParser _parser = new();

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

    [Fact]
    public void Render_EmitsLayoutPost_ForVitePressThemeCompatibility()
    {
        var fm = new FrontMatter("Hello", new DateOnly(2026, 6, 9), ["dotnet"], draft: false, description: "d");

        var rendered = _parser.Render(fm, "# Body");

        rendered.ShouldContain("layout: post");
    }

    [Fact]
    public void Render_EmitsDateAsIsoString_NotDecomposedComponents()
    {
        // YamlDotNet's default DateOnly serialization decomposes into year/month/day/... sub-keys,
        // which VitePress can't read as a date and which won't round-trip. Must be ISO yyyy-MM-dd.
        var fm = new FrontMatter("T", new DateOnly(2026, 6, 9), [], draft: false, description: null);

        var rendered = _parser.Render(fm, "# Body");

        rendered.ShouldContain("date: 2026-06-09");
        rendered.ShouldNotContain("dayofweek");
    }

    [Fact]
    public void Render_RoundTripsThroughParse_WithLayoutAdded()
    {
        var fm = new FrontMatter("Hello World", new DateOnly(2026, 6, 9), ["dotnet", "maui"], draft: false, description: "A post.");

        var roundTripped = _parser.Parse(_parser.Render(fm, "# Body content")).FrontMatter;

        roundTripped.Title.ShouldBe("Hello World");
        roundTripped.Date.ShouldBe(new DateOnly(2026, 6, 9));
        roundTripped.Tags.ShouldBe(["dotnet", "maui"]);
        roundTripped.Draft.ShouldBeFalse();
        roundTripped.Description.ShouldBe("A post.");
    }

    [Fact]
    public void Render_DoesNotDuplicateLayout_WhenParsedDocumentAlreadyHadOne()
    {
        // A document that already carries layout: post must not gain a second one after a round-trip.
        var fm = _parser.Parse(Sample).FrontMatter;

        var rendered = _parser.Render(fm, "# Body");

        Regex.Count(rendered, "layout:").ShouldBe(1);
    }
}
