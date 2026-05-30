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
    public async Task Renders_frontmatter_as_a_table()
    {
        var vm = Create();
        vm.SetMarkdown("---\ntitle: T\n---\n# H");

        await vm.RefreshAsync();

        vm.Html.ShouldContain("<th>title</th>");
        vm.Html.ShouldContain("<h1");
    }
}
