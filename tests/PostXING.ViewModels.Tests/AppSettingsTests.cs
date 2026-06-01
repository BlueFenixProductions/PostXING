using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

public sealed class AppSettingsTests
{
    [Fact]
    public void Default_content_root_lists_top_level_post_folders()
    {
        AppSettings.Default.PostsPrefix.ShouldBe("posts/");
        AppSettings.Default.DraftsPrefix.ShouldBe("drafts/");
    }

    [Theory]
    [InlineData("blog", "blog/posts/", "blog/drafts/")]
    [InlineData("blog/", "blog/posts/", "blog/drafts/")]
    [InlineData("/blog/", "blog/posts/", "blog/drafts/")]
    [InlineData("  blog  ", "blog/posts/", "blog/drafts/")]
    [InlineData("content/site", "content/site/posts/", "content/site/drafts/")]
    public void Content_root_prefixes_post_and_draft_folders(string root, string posts, string drafts)
    {
        var s = AppSettings.Default with { ContentRoot = root };
        s.PostsPrefix.ShouldBe(posts);
        s.DraftsPrefix.ShouldBe(drafts);
    }
}
