using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PostXING.GitHub;

/// <summary>
/// <see cref="IGitHubGateway"/> backed by the GitHub REST API over <see cref="HttpClient"/> — the
/// in-process path used on Android, where the <c>gh</c> CLI can't be shelled out to. Mirrors the
/// endpoints <see cref="GhCliGitHubGateway"/> drives via <c>gh api</c>: a Contents-API PUT is a
/// commit, a pulls POST opens a PR, a merge PUT merges it. The access token is read lazily per
/// request from an <see cref="IGitHubTokenStore"/>; a 401 surfaces as
/// <see cref="GitHubAuthRequiredException"/> so the UI can prompt re-auth.
/// </summary>
public sealed class HttpGitHubGateway(HttpClient http, IGitHubTokenStore tokens, ILogger<HttpGitHubGateway> log) : IGitHubGateway
{
    private static readonly Uri ApiBase = new("https://api.github.com/");
    private const string AcceptMediaType = "application/vnd.github+json";
    private const string ApiVersion = "2022-11-28";
    private const string UserAgent = "PostXING";

    public async Task<string> GetBranchShaAsync(string owner, string repo, string branch, CancellationToken ct = default)
    {
        var (status, body) = await SendAsync(HttpMethod.Get, Endpoint($"repos/{owner}/{repo}/git/ref/heads/{branch}"), null, ct);
        EnsureSuccess(status, body, "get branch sha");
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("object").GetProperty("sha").GetString() ?? string.Empty;
    }

    public async Task CreateBranchAsync(string owner, string repo, string newBranch, string fromSha, CancellationToken ct = default)
    {
        var payload = WriteObject(w =>
        {
            w.WriteString("ref", $"refs/heads/{newBranch}");
            w.WriteString("sha", fromSha);
        });
        var (status, body) = await SendAsync(HttpMethod.Post, Endpoint($"repos/{owner}/{repo}/git/refs"), payload, ct);
        EnsureSuccess(status, body, $"create branch {newBranch}");
        log.LogInformation("Created branch {Branch} from {Sha}", newBranch, fromSha);
    }

    public async Task<string?> GetFileContentAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
    {
        var (status, body) = await SendAsync(HttpMethod.Get, Endpoint($"repos/{owner}/{repo}/contents/{path}?ref={Uri.EscapeDataString(branch)}"), null, ct);
        if (status == HttpStatusCode.NotFound) return null;
        EnsureSuccess(status, body, $"get file {path}");
        using var doc = JsonDocument.Parse(body);
        var encoded = doc.RootElement.GetProperty("content").GetString() ?? string.Empty;
        return Encoding.UTF8.GetString(Convert.FromBase64String(encoded.Replace("\n", string.Empty)));
    }

    public async Task<string?> GetFileShaAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
    {
        var (status, body) = await SendAsync(HttpMethod.Get, Endpoint($"repos/{owner}/{repo}/contents/{path}?ref={Uri.EscapeDataString(branch)}"), null, ct);
        if (status == HttpStatusCode.NotFound) return null;
        EnsureSuccess(status, body, $"get file sha {path}");
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.TryGetProperty("sha", out var sha) ? sha.GetString() : null;
    }

    public async Task UpsertFileAsync(string owner, string repo, string branch, string path, string content, string commitMessage, string? existingFileSha, CancellationToken ct = default)
    {
        var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        var payload = WriteObject(w =>
        {
            w.WriteString("message", commitMessage);
            w.WriteString("content", b64);
            w.WriteString("branch", branch);
            if (existingFileSha is not null) w.WriteString("sha", existingFileSha);
        });
        var (status, body) = await SendAsync(HttpMethod.Put, Endpoint($"repos/{owner}/{repo}/contents/{path}"), payload, ct);
        EnsureSuccess(status, body, $"upsert file {path}");
    }

    public async Task<int> OpenPullRequestAsync(string owner, string repo, string headBranch, string baseBranch, string title, string body, CancellationToken ct = default)
    {
        var payload = WriteObject(w =>
        {
            w.WriteString("title", title);
            w.WriteString("head", headBranch);
            w.WriteString("base", baseBranch);
            w.WriteString("body", body);
        });
        var (status, respBody) = await SendAsync(HttpMethod.Post, Endpoint($"repos/{owner}/{repo}/pulls"), payload, ct);
        EnsureSuccess(status, respBody, "create PR");
        using var doc = JsonDocument.Parse(respBody);
        return doc.RootElement.GetProperty("number").GetInt32();
    }

    public async Task<PullRequestStatus> GetPullRequestStatusAsync(string owner, string repo, int prNumber, CancellationToken ct = default)
    {
        var (status, body) = await SendAsync(HttpMethod.Get, Endpoint($"repos/{owner}/{repo}/pulls/{Num(prNumber)}"), null, ct);
        EnsureSuccess(status, body, $"view PR #{prNumber}");
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        var number = root.GetProperty("number").GetInt32();
        var state = root.GetProperty("state").GetString() ?? string.Empty;
        bool? mergeable = root.TryGetProperty("mergeable", out var m) && m.ValueKind is JsonValueKind.True or JsonValueKind.False
            ? m.GetBoolean()
            : null;
        var mergeableState = root.TryGetProperty("mergeable_state", out var ms) ? ms.GetString() : null;
        // The in-app flow targets a user-chosen branch with no required-checks assumption, so we
        // don't roll up check-runs here; merge is an explicit, unconditional action.
        return new PullRequestStatus(number, state, mergeable, mergeableState, Array.Empty<CheckRunStatus>());
    }

    public async Task<string> MergePullRequestAsync(string owner, string repo, int prNumber, MergeStrategy strategy, CancellationToken ct = default)
    {
        var method = strategy switch
        {
            MergeStrategy.Squash => "squash",
            MergeStrategy.Rebase => "rebase",
            _ => "merge",
        };
        var payload = WriteObject(w => w.WriteString("merge_method", method));
        var (status, body) = await SendAsync(HttpMethod.Put, Endpoint($"repos/{owner}/{repo}/pulls/{Num(prNumber)}/merge"), payload, ct);
        EnsureSuccess(status, body, $"merge PR #{prNumber}");
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.TryGetProperty("sha", out var sha) ? sha.GetString() ?? string.Empty : string.Empty;
    }

    public Task ConfigureBranchProtectionAsync(string owner, string repo, string branch, BranchProtectionRules rules, CancellationToken ct = default) =>
        throw new NotSupportedException("Branch protection is an operator action; configure it on github.com, not from the in-app GitHub client.");

    public async Task<IReadOnlyList<string>> ListMarkdownFilesAsync(string owner, string repo, string branch, string pathPrefix, CancellationToken ct = default)
    {
        var (status, body) = await SendAsync(HttpMethod.Get, Endpoint($"repos/{owner}/{repo}/git/trees/{branch}?recursive=1"), null, ct);
        EnsureSuccess(status, body, $"list tree {branch}");
        using var doc = JsonDocument.Parse(body);
        if (!doc.RootElement.TryGetProperty("tree", out var tree) || tree.ValueKind != JsonValueKind.Array)
            return Array.Empty<string>();

        var result = new List<string>();
        foreach (var entry in tree.EnumerateArray())
        {
            if (entry.TryGetProperty("type", out var t) && t.GetString() != "blob") continue;
            if (!entry.TryGetProperty("path", out var p)) continue;
            var path = p.GetString();
            if (path is null) continue;
            if (!path.StartsWith(pathPrefix, StringComparison.Ordinal)) continue;
            if (!path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)) continue;
            result.Add(path);
        }
        result.Sort(StringComparer.Ordinal);
        return result;
    }

    public async Task<GhAuthStatus> CheckAuthAsync(CancellationToken ct = default)
    {
        var token = await tokens.GetTokenAsync(ct);
        if (string.IsNullOrEmpty(token))
            return new GhAuthStatus(false, null, "No GitHub token set.");

        var (status, body) = await SendAsync(HttpMethod.Get, Endpoint("user"), null, ct);
        if (status == HttpStatusCode.Unauthorized)
            return new GhAuthStatus(false, null, "GitHub token is invalid or expired.");
        if (!IsSuccess(status))
            return new GhAuthStatus(false, null, $"GitHub auth check failed (HTTP {(int)status}).");

        using var doc = JsonDocument.Parse(body);
        var login = doc.RootElement.TryGetProperty("login", out var l) ? l.GetString() : null;
        return new GhAuthStatus(true, login, login is null ? "Authenticated." : $"Authenticated as {login}.");
    }

    private static Uri Endpoint(string relative) => new(ApiBase, relative);

    private static string Num(int n) => n.ToString(CultureInfo.InvariantCulture);

    private async Task<(HttpStatusCode Status, string Body)> SendAsync(HttpMethod method, Uri uri, string? jsonBody, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(method, uri);
        request.Headers.TryAddWithoutValidation("Accept", AcceptMediaType);
        request.Headers.TryAddWithoutValidation("User-Agent", UserAgent);
        request.Headers.TryAddWithoutValidation("X-GitHub-Api-Version", ApiVersion);

        var token = await tokens.GetTokenAsync(ct);
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (jsonBody is not null)
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        using var response = await http.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);
        return (response.StatusCode, body);
    }

    private static bool IsSuccess(HttpStatusCode status) => (int)status is >= 200 and < 300;

    private static void EnsureSuccess(HttpStatusCode status, string body, string op)
    {
        if (IsSuccess(status)) return;
        if (status == HttpStatusCode.Unauthorized)
            throw new GitHubAuthRequiredException($"GitHub {op} requires authentication (HTTP 401): {body.Trim()}");
        throw new InvalidOperationException($"GitHub {op} failed (HTTP {(int)status}): {body.Trim()}");
    }

    private static string WriteObject(Action<Utf8JsonWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            write(writer);
            writer.WriteEndObject();
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
