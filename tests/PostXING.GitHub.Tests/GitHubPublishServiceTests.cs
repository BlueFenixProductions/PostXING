using PostXING.Core.Domain;
using PostXING.GitHub;
using Shouldly;
using Xunit;

namespace PostXING.GitHub.Tests;

public sealed class GitHubPublishServiceTests
{
    private readonly InMemoryGitHubGateway _gw = new();
    private readonly SiteConfig _site = SiteConfig.For("owner", "repo");

    private static Post SamplePost() =>
        new(Slug.From("Hello World"),
            FrontMatter.Default.WithTitle("Hello World"),
            "# Hello",
            new DateOnly(2025, 6, 1));

    [Fact]
    public async Task PublishAsync_CreatesBranchOpensPullRequest()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");
        var sut = new GitHubPublishService(_gw);

        var state = await sut.PublishAsync(SamplePost(), _site, "rendered", autoMerge: false);

        state.Kind.ShouldBe(PublishKind.PullRequestOpen);
        state.BranchName.ShouldStartWith("post/hello-world-");
        state.PullRequestNumber.ShouldNotBeNull();
    }

    [Fact]
    public async Task PublishAsync_default_writes_to_top_level_posts()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");

        var state = await new GitHubPublishService(_gw).PublishAsync(SamplePost(), _site, "rendered", autoMerge: false);

        var files = await _gw.ListMarkdownFilesAsync("owner", "repo", state.BranchName!, "posts/");
        files.ShouldHaveSingleItem().ShouldStartWith("posts/");
    }

    [Fact]
    public async Task PublishAsync_writes_under_the_content_root_when_set()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");
        var site = SiteConfig.For("owner", "repo") with { ContentRoot = "blog" };

        var state = await new GitHubPublishService(_gw).PublishAsync(SamplePost(), site, "rendered", autoMerge: false);

        var path = (await _gw.ListMarkdownFilesAsync("owner", "repo", state.BranchName!, "blog/posts/")).ShouldHaveSingleItem();
        path.ShouldStartWith("blog/posts/");
        path.ShouldEndWith("-hello-world.md");
    }

    [Fact]
    public async Task SaveDraftAsync_writes_under_the_content_root_when_set()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");
        var site = SiteConfig.For("owner", "repo") with { ContentRoot = "blog" };

        await new GitHubPublishService(_gw).SaveDraftAsync(SamplePost(), site, "rendered");

        var files = await _gw.ListMarkdownFilesAsync("owner", "repo", "develop", "blog/drafts/");
        files.ShouldHaveSingleItem().ShouldBe("blog/drafts/hello-world.md");
    }
}
