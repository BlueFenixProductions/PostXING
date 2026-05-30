using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PostXING.GitHub;

public sealed class GhCliGitHubGateway(ILogger<GhCliGitHubGateway> log, string ghPath = "gh") : IGitHubGateway
{
    private readonly string _gh = ghPath;
    private readonly ILogger<GhCliGitHubGateway> _log = log;

    public async Task<string> GetBranchShaAsync(string owner, string repo, string branch, CancellationToken ct = default)
    {
        var (exit, stdout, stderr) = await RunAsync(
            ["api", $"repos/{owner}/{repo}/git/ref/heads/{branch}", "--jq", ".object.sha"],
            stdin: null, ct);
        if (exit != 0) throw GhException("get branch sha", exit, stderr);
        return stdout.Trim();
    }

    public async Task CreateBranchAsync(string owner, string repo, string newBranch, string fromSha, CancellationToken ct = default)
    {
        var (exit, _, stderr) = await RunAsync(
            ["api", "-X", "POST", $"repos/{owner}/{repo}/git/refs",
             "-f", $"ref=refs/heads/{newBranch}",
             "-f", $"sha={fromSha}"],
            stdin: null, ct);
        if (exit != 0) throw GhException($"create branch {newBranch}", exit, stderr);
        _log.LogInformation("Created branch {Branch} from {Sha}", newBranch, fromSha);
    }

    public async Task<string?> GetFileContentAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
    {
        var (exit, stdout, stderr) = await RunAsync(
            ["api", $"repos/{owner}/{repo}/contents/{path}?ref={branch}"],
            stdin: null, ct);
        if (exit != 0)
        {
            if (IsNotFound(stderr)) return null;
            throw GhException($"get file {path}", exit, stderr);
        }
        using var doc = JsonDocument.Parse(stdout);
        var encoded = doc.RootElement.GetProperty("content").GetString() ?? string.Empty;
        return Encoding.UTF8.GetString(Convert.FromBase64String(encoded.Replace("\n", string.Empty)));
    }

    public async Task<string?> GetFileShaAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
    {
        var (exit, stdout, stderr) = await RunAsync(
            ["api", $"repos/{owner}/{repo}/contents/{path}?ref={branch}", "--jq", ".sha"],
            stdin: null, ct);
        if (exit != 0)
        {
            if (IsNotFound(stderr)) return null;
            throw GhException($"get file sha {path}", exit, stderr);
        }
        var sha = stdout.Trim();
        return string.IsNullOrEmpty(sha) ? null : sha;
    }

    public async Task UpsertFileAsync(string owner, string repo, string branch, string path, string content, string commitMessage, string? existingFileSha, CancellationToken ct = default)
    {
        var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        var args = new List<string>
        {
            "api", "-X", "PUT", $"repos/{owner}/{repo}/contents/{path}",
            "-f", $"message={commitMessage}",
            "-f", $"content={b64}",
            "-f", $"branch={branch}",
        };
        if (existingFileSha is not null)
        {
            args.Add("-f");
            args.Add($"sha={existingFileSha}");
        }
        var (exit, _, stderr) = await RunAsync(args.ToArray(), stdin: null, ct);
        if (exit != 0) throw GhException($"upsert file {path}", exit, stderr);
    }

    public async Task<int> OpenPullRequestAsync(string owner, string repo, string headBranch, string baseBranch, string title, string body, CancellationToken ct = default)
    {
        var (exit, stdout, stderr) = await RunAsync(
            ["pr", "create",
             "--repo", $"{owner}/{repo}",
             "--base", baseBranch,
             "--head", headBranch,
             "--title", title,
             "--body", body],
            stdin: null, ct);
        if (exit != 0) throw GhException("create PR", exit, stderr);
        return ParsePrNumberFromUrl(stdout.Trim());
    }

    public async Task<PullRequestStatus> GetPullRequestStatusAsync(string owner, string repo, int prNumber, CancellationToken ct = default)
    {
        var (exit, stdout, stderr) = await RunAsync(
            ["pr", "view", prNumber.ToString(System.Globalization.CultureInfo.InvariantCulture),
             "--repo", $"{owner}/{repo}",
             "--json", "number,state,mergeable,mergeStateStatus,statusCheckRollup"],
            stdin: null, ct);
        if (exit != 0) throw GhException($"view PR #{prNumber}", exit, stderr);

        using var doc = JsonDocument.Parse(stdout);
        var root = doc.RootElement;
        var number = root.GetProperty("number").GetInt32();
        var state = root.GetProperty("state").GetString() ?? string.Empty;
        var mergeable = TryGetMergeable(root);
        var mergeStateStatus = root.TryGetProperty("mergeStateStatus", out var mss) ? mss.GetString() : null;
        var checks = ParseCheckRollup(root);
        return new PullRequestStatus(number, state, mergeable, mergeStateStatus, checks);
    }

    public async Task<string> MergePullRequestAsync(string owner, string repo, int prNumber, MergeStrategy strategy, CancellationToken ct = default)
    {
        var flag = strategy switch
        {
            MergeStrategy.Squash => "--squash",
            MergeStrategy.Rebase => "--rebase",
            _ => "--merge",
        };
        var (exit, _, stderr) = await RunAsync(
            ["pr", "merge", prNumber.ToString(System.Globalization.CultureInfo.InvariantCulture), "--repo", $"{owner}/{repo}", flag],
            stdin: null, ct);
        if (exit != 0) throw GhException($"merge PR #{prNumber}", exit, stderr);

        var (exit2, stdout2, stderr2) = await RunAsync(
            ["pr", "view", prNumber.ToString(System.Globalization.CultureInfo.InvariantCulture), "--repo", $"{owner}/{repo}", "--json", "mergeCommit"],
            stdin: null, ct);
        if (exit2 != 0) throw GhException($"read merge commit for PR #{prNumber}", exit2, stderr2);

        using var doc = JsonDocument.Parse(stdout2);
        return doc.RootElement.GetProperty("mergeCommit").GetProperty("oid").GetString() ?? string.Empty;
    }

    public async Task<IReadOnlyList<string>> ListMarkdownFilesAsync(string owner, string repo, string branch, string pathPrefix, CancellationToken ct = default)
    {
        var (exit, stdout, stderr) = await RunAsync(
            ["api", $"repos/{owner}/{repo}/git/trees/{branch}?recursive=1"],
            stdin: null, ct);
        if (exit != 0) throw GhException($"list tree {branch}", exit, stderr);

        using var doc = JsonDocument.Parse(stdout);
        if (!doc.RootElement.TryGetProperty("tree", out var tree) || tree.ValueKind != JsonValueKind.Array)
            return Array.Empty<string>();

        var result = new List<string>();
        foreach (var entry in tree.EnumerateArray())
        {
            if (entry.GetProperty("type").GetString() != "blob") continue;
            var path = entry.GetProperty("path").GetString();
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
        try
        {
            var (exit, stdout, stderr) = await RunAsync(["auth", "status"], stdin: null, ct);
            var combined = string.IsNullOrEmpty(stdout) ? stderr : stdout;
            if (exit != 0)
                return new GhAuthStatus(false, null, combined.Trim());

            string? user = null;
            foreach (var line in combined.Split('\n'))
            {
                var idx = line.IndexOf("account ", StringComparison.OrdinalIgnoreCase);
                if (idx < 0) continue;
                var tail = line[(idx + "account ".Length)..].Trim();
                var space = tail.IndexOf(' ');
                user = space > 0 ? tail[..space] : tail;
                break;
            }
            return new GhAuthStatus(true, user, combined.Trim());
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return new GhAuthStatus(false, null, "gh CLI not on PATH");
        }
    }

    public async Task ConfigureBranchProtectionAsync(string owner, string repo, string branch, BranchProtectionRules rules, CancellationToken ct = default)
    {
        var payload = BuildProtectionPayload(rules);
        var (exit, _, stderr) = await RunAsync(
            ["api", "-X", "PUT", $"repos/{owner}/{repo}/branches/{branch}/protection", "--input", "-"],
            stdin: payload, ct);
        if (exit != 0) throw GhException($"configure protection for {branch}", exit, stderr);
    }

    private static string BuildProtectionPayload(BranchProtectionRules rules)
    {
        var contexts = string.Join(",", rules.RequiredStatusChecks.Select(c => $"\"{c}\""));
        var restrictionsJson = rules.RestrictPushesToAdminsOnly
            ? $$"""
                "restrictions": { "users": [], "teams": [{{string.Join(",", (rules.AllowedPushTeams ?? []).Select(t => $"\"{t}\""))}}], "apps": [] }
                """
            : "\"restrictions\": null";

        return $$"""
            {
              "required_status_checks": { "strict": true, "contexts": [{{contexts}}] },
              "enforce_admins": true,
              "required_pull_request_reviews": {
                "dismiss_stale_reviews": true,
                "require_code_owner_reviews": false,
                "required_approving_review_count": {{rules.RequiredApprovingReviews}}
              },
              {{restrictionsJson}},
              "required_linear_history": {{(rules.RequireLinearHistory ? "true" : "false")}},
              "allow_force_pushes": false,
              "allow_deletions": false,
              "required_conversation_resolution": true
            }
            """;
    }

    private static bool? TryGetMergeable(JsonElement root)
    {
        if (!root.TryGetProperty("mergeable", out var m)) return null;
        return m.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String => m.GetString() switch
            {
                "MERGEABLE" => true,
                "CONFLICTING" => false,
                _ => null,
            },
            _ => null,
        };
    }

    private static IReadOnlyList<CheckRunStatus> ParseCheckRollup(JsonElement root)
    {
        if (!root.TryGetProperty("statusCheckRollup", out var rollup) || rollup.ValueKind != JsonValueKind.Array)
            return Array.Empty<CheckRunStatus>();
        var list = new List<CheckRunStatus>();
        foreach (var c in rollup.EnumerateArray())
        {
            var name = c.TryGetProperty("name", out var n) ? n.GetString() ?? "(unnamed)" : "(unnamed)";
            var status = c.TryGetProperty("status", out var s) ? s.GetString() ?? string.Empty : string.Empty;
            var conclusion = c.TryGetProperty("conclusion", out var co) ? co.GetString() : null;
            list.Add(new CheckRunStatus(name, status, conclusion));
        }
        return list;
    }

    private static int ParsePrNumberFromUrl(string url)
    {
        var slash = url.LastIndexOf('/');
        if (slash < 0 || slash == url.Length - 1)
            throw new InvalidOperationException($"Could not parse PR number from `gh pr create` output: {url}");
        return int.Parse(url[(slash + 1)..], System.Globalization.CultureInfo.InvariantCulture);
    }

    private static bool IsNotFound(string stderr) =>
        stderr.Contains("HTTP 404", StringComparison.OrdinalIgnoreCase)
        || stderr.Contains("Not Found", StringComparison.OrdinalIgnoreCase);

    private static InvalidOperationException GhException(string op, int exit, string stderr) =>
        new($"`gh` {op} failed (exit {exit}): {stderr.Trim()}");

    private async Task<(int exit, string stdout, string stderr)> RunAsync(string[] args, string? stdin, CancellationToken ct)
    {
        var psi = new ProcessStartInfo(_gh)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = stdin is not null,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (var a in args) psi.ArgumentList.Add(a);

        using var p = new Process { StartInfo = psi };
        if (!p.Start())
            throw new InvalidOperationException($"Failed to launch `{_gh}`. Is the GitHub CLI installed and on PATH?");

        if (stdin is not null)
        {
            await p.StandardInput.WriteAsync(stdin);
            p.StandardInput.Close();
        }

        var stdoutTask = p.StandardOutput.ReadToEndAsync(ct);
        var stderrTask = p.StandardError.ReadToEndAsync(ct);
        await p.WaitForExitAsync(ct);
        return (p.ExitCode, await stdoutTask, await stderrTask);
    }
}
