using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;

namespace PostXING.GitHub.Tests;

public sealed class HttpGitHubGatewayTests
{
    private static HttpGitHubGateway Make(FakeHttpMessageHandler handler, string? token = "ghp_test") =>
        new(new HttpClient(handler), new InMemoryGitHubTokenStore(token), NullLogger<HttpGitHubGateway>.Instance);

    // Parse the recorded request body back to JSON so assertions don't depend on serializer
    // whitespace/escaping. The returned element keeps its JsonDocument alive.
    private static JsonElement BodyOf(CapturedRequest req)
    {
        req.Body.ShouldNotBeNull();
        return JsonDocument.Parse(req.Body).RootElement;
    }

    [Fact]
    public async Task GetBranchSha_calls_ref_endpoint_with_standard_headers()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK, """{"object":{"sha":"abc123"}}""");

        var sha = await Make(handler).GetBranchShaAsync("owner", "repo", "develop");

        sha.ShouldBe("abc123");
        var req = handler.Requests.ShouldHaveSingleItem();
        req.Method.ShouldBe(HttpMethod.Get);
        req.Path.ShouldBe("/repos/owner/repo/git/ref/heads/develop");
        req.Authorization.ShouldBe("Bearer ghp_test");
        req.Header("Accept").ShouldBe("application/vnd.github+json");
        req.Header("X-GitHub-Api-Version").ShouldBe("2022-11-28");
        req.Header("User-Agent").ShouldBe("PostXING");
    }

    [Fact]
    public async Task CreateBranch_posts_ref_and_sha()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.Created);

        await Make(handler).CreateBranchAsync("o", "r", "post/hello-20260601", "base999");

        var req = handler.Requests.ShouldHaveSingleItem();
        req.Method.ShouldBe(HttpMethod.Post);
        req.Path.ShouldBe("/repos/o/r/git/refs");
        var body = BodyOf(req);
        body.GetProperty("ref").GetString().ShouldBe("refs/heads/post/hello-20260601");
        body.GetProperty("sha").GetString().ShouldBe("base999");
    }

    [Fact]
    public async Task GetFileContent_base64_decodes_and_passes_ref()
    {
        var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("# Hello"));
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK, $$"""{"content":"{{b64}}","sha":"s1"}""");

        var content = await Make(handler).GetFileContentAsync("o", "r", "develop", "posts/x.md");

        content.ShouldBe("# Hello");
        handler.Requests[0].Path.ShouldBe("/repos/o/r/contents/posts/x.md");
        handler.Requests[0].Query.ShouldBe("?ref=develop");
    }

    [Fact]
    public async Task GetFileContent_returns_null_on_404()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.NotFound, """{"message":"Not Found"}""");
        (await Make(handler).GetFileContentAsync("o", "r", "develop", "missing.md")).ShouldBeNull();
    }

    [Fact]
    public async Task GetFileSha_returns_blob_sha()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK, """{"sha":"blob42"}""");
        (await Make(handler).GetFileShaAsync("o", "r", "develop", "drafts/a.md")).ShouldBe("blob42");
    }

    [Fact]
    public async Task GetFileSha_returns_null_on_404()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.NotFound);
        (await Make(handler).GetFileShaAsync("o", "r", "develop", "drafts/a.md")).ShouldBeNull();
    }

    [Fact]
    public async Task UpsertFile_includes_sha_branch_and_base64_content_when_updating()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK);

        await Make(handler).UpsertFileAsync("o", "r", "develop", "drafts/a.md", "body", "edit: a.md", "oldsha");

        var req = handler.Requests.ShouldHaveSingleItem();
        req.Method.ShouldBe(HttpMethod.Put);
        req.Path.ShouldBe("/repos/o/r/contents/drafts/a.md");
        var body = BodyOf(req);
        body.GetProperty("sha").GetString().ShouldBe("oldsha");
        body.GetProperty("branch").GetString().ShouldBe("develop");
        body.GetProperty("message").GetString().ShouldBe("edit: a.md");
        body.GetProperty("content").GetString().ShouldBe(Convert.ToBase64String(Encoding.UTF8.GetBytes("body")));
    }

    [Fact]
    public async Task UpsertFile_omits_sha_when_creating()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.Created);

        await Make(handler).UpsertFileAsync("o", "r", "develop", "drafts/new.md", "body", "draft: new", null);

        BodyOf(handler.Requests[0]).TryGetProperty("sha", out _).ShouldBeFalse();
    }

    [Fact]
    public async Task OpenPullRequest_posts_head_base_and_returns_number()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.Created, """{"number":42}""");

        var n = await Make(handler).OpenPullRequestAsync("o", "r", "post/x", "develop", "Post: X", "the body");

        n.ShouldBe(42);
        var req = handler.Requests.ShouldHaveSingleItem();
        req.Path.ShouldBe("/repos/o/r/pulls");
        var body = BodyOf(req);
        body.GetProperty("head").GetString().ShouldBe("post/x");
        body.GetProperty("base").GetString().ShouldBe("develop");
        body.GetProperty("title").GetString().ShouldBe("Post: X");
    }

    [Fact]
    public async Task MergePullRequest_squash_and_returns_sha()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK, """{"sha":"merged99","merged":true}""");

        var sha = await Make(handler).MergePullRequestAsync("o", "r", 42, MergeStrategy.Squash);

        sha.ShouldBe("merged99");
        var req = handler.Requests.ShouldHaveSingleItem();
        req.Method.ShouldBe(HttpMethod.Put);
        req.Path.ShouldBe("/repos/o/r/pulls/42/merge");
        BodyOf(req).GetProperty("merge_method").GetString().ShouldBe("squash");
    }

    [Fact]
    public async Task GetPullRequestStatus_maps_state_and_mergeable_with_no_checks()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK,
            """{"number":42,"state":"open","mergeable":true,"mergeable_state":"clean"}""");

        var status = await Make(handler).GetPullRequestStatusAsync("o", "r", 42);

        status.Number.ShouldBe(42);
        status.State.ShouldBe("open");
        status.Mergeable.ShouldBe(true);
        status.MergeableState.ShouldBe("clean");
        status.Checks.ShouldBeEmpty();
    }

    [Fact]
    public async Task ListMarkdownFiles_filters_prefix_and_md_and_sorts_ordinal()
    {
        const string tree = """{"tree":[{"path":"posts/2026-06-01-b.md","type":"blob"},{"path":"posts/2026-05-01-a.md","type":"blob"},{"path":"posts/img.png","type":"blob"},{"path":"drafts/x.md","type":"blob"},{"path":"posts","type":"tree"}]}""";
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK, tree);

        var files = await Make(handler).ListMarkdownFilesAsync("o", "r", "develop", "posts/");

        files.Count.ShouldBe(2);
        files[0].ShouldBe("posts/2026-05-01-a.md");   // ordinal sort: 05 before 06
        files[1].ShouldBe("posts/2026-06-01-b.md");
        handler.Requests[0].Path.ShouldBe("/repos/o/r/git/trees/develop");
        handler.Requests[0].Query.ShouldContain("recursive=1");
    }

    [Fact]
    public async Task CheckAuth_without_token_is_unauthenticated_and_makes_no_request()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK);

        var status = await Make(handler, token: null).CheckAuthAsync();

        status.IsAuthenticated.ShouldBeFalse();
        handler.Requests.ShouldBeEmpty();
    }

    [Fact]
    public async Task CheckAuth_with_valid_token_returns_login()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK, """{"login":"chris"}""");

        var status = await Make(handler).CheckAuthAsync();

        status.IsAuthenticated.ShouldBeTrue();
        status.Username.ShouldBe("chris");
        handler.Requests[0].Path.ShouldBe("/user");
    }

    [Fact]
    public async Task CheckAuth_with_invalid_token_is_unauthenticated()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.Unauthorized, """{"message":"Bad credentials"}""");

        var status = await Make(handler).CheckAuthAsync();

        status.IsAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public async Task Unauthorized_on_a_call_throws_auth_required()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.Unauthorized, """{"message":"Bad credentials"}""");
        await Should.ThrowAsync<GitHubAuthRequiredException>(() => Make(handler).GetBranchShaAsync("o", "r", "develop"));
    }

    [Fact]
    public async Task Server_error_throws_invalid_operation_with_status_code()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.InternalServerError, "boom");
        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => Make(handler).OpenPullRequestAsync("o", "r", "h", "b", "t", "x"));
        ex.Message.ShouldContain("500");
    }

    [Fact]
    public async Task ConfigureBranchProtection_is_not_supported_in_app()
    {
        var handler = FakeHttpMessageHandler.Respond(HttpStatusCode.OK);
        await Should.ThrowAsync<NotSupportedException>(() =>
            Make(handler).ConfigureBranchProtectionAsync("o", "r", "develop",
                new BranchProtectionRules(Array.Empty<string>(), 0, false, false, null)));
    }
}
