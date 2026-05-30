using PostXING.Markdown;
using Shouldly;
using Xunit;

namespace PostXING.Markdown.Tests;

public sealed class PreviewRendererTests
{
    private readonly PreviewRenderer _sut = new();

    [Fact]
    public void Body_renders_inside_markdown_body_wrapper()
    {
        var html = _sut.Build("# Hello\n\nWorld **bold**.", "/* css */");
        html.ShouldContain("class=\"markdown-body\"");
        html.ShouldContain("<h1");
        html.ShouldContain("<strong>bold</strong>");
    }

    [Fact]
    public void Bundled_github_css_is_inlined()
    {
        _sut.Build("x", "GITHUB_MARKDOWN_CSS_MARKER").ShouldContain("GITHUB_MARKDOWN_CSS_MARKER");
    }

    [Fact]
    public void Inline_html_in_body_is_preserved()
    {
        _sut.Build("a <br/> b", "").ShouldContain("<br");
    }

    [Fact]
    public void Frontmatter_renders_as_a_table_with_all_keys_in_order()
    {
        var html = _sut.Build("---\ntitle: Hello\nauthor: Chris\ncustomKey: yep\n---\n\nbody", "");
        html.ShouldContain("<table>");
        html.ShouldContain("<th>title</th>");
        html.ShouldContain("<th>author</th>");
        html.ShouldContain("<th>customKey</th>");   // arbitrary keys preserved, as GitHub-style th cells
        html.IndexOf("<th>title</th>", StringComparison.Ordinal)
            .ShouldBeLessThan(html.IndexOf("<th>author</th>", StringComparison.Ordinal));
        html.IndexOf("<th>author</th>", StringComparison.Ordinal)
            .ShouldBeLessThan(html.IndexOf("<th>customKey</th>", StringComparison.Ordinal));
    }

    [Fact]
    public void Frontmatter_scalar_list_renders_space_joined_in_one_cell()
    {
        // Matches github.com: a list value sits in one cell, items space-separated (clean 2-col table).
        _sut.Build("---\ntags:\n  - dotnet\n  - maui\n---\n\nbody", "")
            .ShouldContain("<td>dotnet maui</td>");
    }

    [Fact]
    public void No_frontmatter_means_no_table()
    {
        var html = _sut.Build("just body, no frontmatter", "");
        html.ShouldNotContain("<table>");
        html.ShouldContain("just body");
    }

    [Fact]
    public void Dark_uses_dark_canvas_and_light_table_text_light_uses_the_inverse()
    {
        var dark = _sut.Build("x", "", dark: true);
        dark.ShouldContain("#0d1117");                       // dark canvas
        dark.ShouldContain("table th,.markdown-body table td{color:#f0f6fc");  // light table text

        var light = _sut.Build("x", "", dark: false);
        light.ShouldContain("#ffffff");                      // light canvas
        light.ShouldContain("table th,.markdown-body table td{color:#1f2328"); // dark table text
    }
}
