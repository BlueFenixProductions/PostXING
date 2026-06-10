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

    // gh #48: re-publishing an already-published post must update its ORIGINAL file, not mint a
    // new today-dated duplicate. The caller passes the existing path; the date/slug in it win.
    [Fact]
    public async Task PublishAsync_with_an_existing_path_writes_to_that_path_preserving_the_permalink()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");

        // Post's slug is "hello-world", but the original path carries a different date AND slug —
        // the original path must be honored verbatim (permalink stability beats slug-match).
        var state = await new GitHubPublishService(_gw, new FixedClock(new DateTimeOffset(2026, 6, 10, 12, 0, 0, TimeSpan.Zero)))
            .PublishAsync(SamplePost(), _site, "rendered", autoMerge: false,
                existingPostPath: "posts/2024-01-15-original-slug.md");

        var files = await _gw.ListMarkdownFilesAsync("owner", "repo", state.BranchName!, "posts/");
        files.ShouldHaveSingleItem().ShouldBe("posts/2024-01-15-original-slug.md");
    }

    [Fact]
    public async Task PublishAsync_without_an_existing_path_still_mints_a_today_dated_path()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");

        var state = await new GitHubPublishService(_gw, new FixedClock(new DateTimeOffset(2026, 6, 10, 12, 0, 0, TimeSpan.Zero)))
            .PublishAsync(SamplePost(), _site, "rendered", autoMerge: false);

        var files = await _gw.ListMarkdownFilesAsync("owner", "repo", state.BranchName!, "posts/");
        files.ShouldHaveSingleItem().ShouldBe("posts/2026-06-10-hello-world.md");
    }

    [Fact]
    public async Task Same_day_republishes_get_distinct_branch_names()
    {
        await _gw.SeedBranchAsync("owner", "repo", "develop", "abc123");
        var t = new DateTimeOffset(2026, 6, 10, 12, 0, 0, TimeSpan.Zero);

        var a = await new GitHubPublishService(_gw, new FixedClock(t))
            .PublishAsync(SamplePost(), _site, "r", autoMerge: false, existingPostPath: "posts/2024-01-15-x.md");
        var b = await new GitHubPublishService(_gw, new FixedClock(t.AddSeconds(1)))
            .PublishAsync(SamplePost(), _site, "r", autoMerge: false, existingPostPath: "posts/2024-01-15-x.md");

        a.BranchName.ShouldNotBe(b.BranchName);
    }

    private sealed class FixedClock(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }
}
