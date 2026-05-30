using System.Collections.Concurrent;

namespace PostXING.GitHub;

public sealed class InMemoryGitHubGateway : IGitHubGateway
{
    private readonly ConcurrentDictionary<(string owner, string repo, string branch), string> _branches = new();
    private readonly ConcurrentDictionary<(string owner, string repo, string branch, string path), string> _files = new();
    private readonly ConcurrentDictionary<int, (string owner, string repo, string head, string @base, string title)> _prs = new();
    private int _nextPr = 100;

    public Task SeedBranchAsync(string owner, string repo, string branch, string sha)
    {
        _branches[(owner, repo, branch)] = sha;
        return Task.CompletedTask;
    }

    public Task<string> GetBranchShaAsync(string owner, string repo, string branch, CancellationToken ct = default)
        => Task.FromResult(_branches[(owner, repo, branch)]);

    public Task CreateBranchAsync(string owner, string repo, string newBranch, string fromSha, CancellationToken ct = default)
    {
        _branches[(owner, repo, newBranch)] = fromSha;
        return Task.CompletedTask;
    }

    public Task<string?> GetFileContentAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
        => Task.FromResult(_files.TryGetValue((owner, repo, branch, path), out var c) ? c : null);

    public Task<string?> GetFileShaAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
    {
        // The real GitHub sha is the git blob hash of the content; for the in-memory test
        // fake, a stable deterministic surrogate is enough — callers only round-trip it.
        if (!_files.TryGetValue((owner, repo, branch, path), out var c)) return Task.FromResult<string?>(null);
        var hash = $"{(uint)c.GetHashCode():x8}{(uint)path.GetHashCode():x8}";
        return Task.FromResult<string?>(hash);
    }

    public Task UpsertFileAsync(string owner, string repo, string branch, string path, string content, string commitMessage, string? existingFileSha, CancellationToken ct = default)
    {
        _files[(owner, repo, branch, path)] = content;
        return Task.CompletedTask;
    }

    public Task<int> OpenPullRequestAsync(string owner, string repo, string headBranch, string baseBranch, string title, string body, CancellationToken ct = default)
    {
        var num = Interlocked.Increment(ref _nextPr);
        _prs[num] = (owner, repo, headBranch, baseBranch, title);
        return Task.FromResult(num);
    }

    public Task<PullRequestStatus> GetPullRequestStatusAsync(string owner, string repo, int prNumber, CancellationToken ct = default)
        => Task.FromResult(new PullRequestStatus(prNumber, "open", true, "clean", Array.Empty<CheckRunStatus>()));

    public Task<string> MergePullRequestAsync(string owner, string repo, int prNumber, MergeStrategy strategy, CancellationToken ct = default)
        => Task.FromResult($"merged-{prNumber:x8}");

    public Task ConfigureBranchProtectionAsync(string owner, string repo, string branch, BranchProtectionRules rules, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task<IReadOnlyList<string>> ListMarkdownFilesAsync(string owner, string repo, string branch, string pathPrefix, CancellationToken ct = default)
    {
        var list = _files
            .Where(kvp => kvp.Key.owner == owner && kvp.Key.repo == repo && kvp.Key.branch == branch
                          && kvp.Key.path.StartsWith(pathPrefix, StringComparison.Ordinal)
                          && kvp.Key.path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            .Select(kvp => kvp.Key.path)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
        return Task.FromResult<IReadOnlyList<string>>(list);
    }

    public Task<GhAuthStatus> CheckAuthAsync(CancellationToken ct = default)
        => Task.FromResult(new GhAuthStatus(true, "test-user", "in-memory gateway"));
}
