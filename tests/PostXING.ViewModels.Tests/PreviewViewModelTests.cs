using NSubstitute;
using PostXING.Markdown;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

public sealed class PreviewViewModelTests
{
    private static PreviewViewModel Create()
    {
        var styles = Substitute.For<IPreviewStyles>();
        styles.GithubMarkdownCss(true).Returns("DARK_GITHUB_CSS");
        styles.GithubMarkdownCss(false).Returns("LIGHT_GITHUB_CSS");
        return new PreviewViewModel(new PreviewRenderer(), styles);
    }

    [Fact]
    public async Task Dark_render_uses_dark_css_and_canvas()
    {
        var vm = Create();
        vm.SetMarkdown("# Hi");
        vm.Dark = true;

        await vm.RefreshAsync();

        vm.Html.ShouldContain("DARK_GITHUB_CSS");
        vm.Html.ShouldContain("#0d1117");
        vm.Html.ShouldContain("markdown-body");
    }

    [Fact]
    public async Task Light_render_uses_light_css_and_canvas()
    {
        var vm = Create();
        vm.SetMarkdown("# Hi");
        vm.Dark = false;

        await vm.RefreshAsync();

        vm.Html.ShouldContain("LIGHT_GITHUB_CSS");
        vm.Html.ShouldContain("#ffffff");
    }

    [Fact]
    public async Task Themed_render_uses_the_theme_preview_palette()
    {
        var vm = Create();
        vm.SetMarkdown("# Hi");
        vm.Theme = PostXING.ViewModels.Theming.ThemeCatalog.Get("tokyo-night");

        await vm.RefreshAsync();

        var p = vm.Theme.Preview;
        vm.Html.ShouldContain(p.Canvas);
        vm.Html.ShouldContain(p.Fg);
        vm.Html.ShouldContain(p.Link);
    }

    [Fact]
    public async Task Dark_render_wires_the_dark_hljs_library_and_theme()
    {
        var styles = Substitute.For<IPreviewStyles>();
        styles.GithubMarkdownCss(true).Returns("DARK_GITHUB_CSS");
        styles.HighlightJs().Returns("HLJS_LIBRARY");
        styles.HighlightThemeCss(true).Returns("HLJS_DARK_THEME");
        var vm = new PreviewViewModel(new PreviewRenderer(), styles);
        vm.SetMarkdown("```csharp\nvar x = 1;\n```");
        vm.Dark = true;

        await vm.RefreshAsync();

        vm.Html.ShouldContain("HLJS_LIBRARY");
        vm.Html.ShouldContain("HLJS_DARK_THEME");
        vm.Html.ShouldContain("hljs.highlightAll");
        vm.Html.ShouldContain("language-csharp");
    }

    [Fact]
    public async Task Renders_frontmatter_as_a_table()
    {
        var vm = Create();
        vm.SetMarkdown("---\ntitle: T\n---\n# H");

        await vm.RefreshAsync();

        vm.Html.ShouldContain("<th>title</th>");
        vm.Html.ShouldContain("<h1");
    }
}
