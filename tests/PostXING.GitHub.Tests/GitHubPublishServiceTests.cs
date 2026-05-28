using PostXING.Core.Domain;
using PostXING.GitHub;
using Shouldly;
using Xunit;

namespace PostXING.GitHub.Tests;

public sealed class GitHubPublishServiceTests
{
    private readonly InMemoryGitHubGateway _gw = new();
    private readonly SiteConfig _site = SiteConfig.For("owner", "repo", "content/posts", SiteFlavor.Hugo);

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
        state.BranchName.ShouldBe("post/hello-world-20250601");
        state.PullRequestNumber.ShouldNotBeNull();
    }
}
