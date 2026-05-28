namespace PostXING.GitHub;

public enum MergeStrategy { Merge, Squash, Rebase }

public sealed record CheckRunStatus(string Name, string Status, string? Conclusion);

public sealed record PullRequestStatus(
    int Number,
    string State,
    bool? Mergeable,
    string? MergeableState,
    IReadOnlyList<CheckRunStatus> Checks);

public sealed record BranchProtectionRules(
    IReadOnlyList<string> RequiredStatusChecks,
    int RequiredApprovingReviews,
    bool RequireLinearHistory,
    bool RestrictPushesToAdminsOnly,
    IReadOnlyList<string>? AllowedPushTeams);

public interface IGitHubGateway
{
    Task<string> GetBranchShaAsync(string owner, string repo, string branch, CancellationToken ct = default);
    Task CreateBranchAsync(string owner, string repo, string newBranch, string fromSha, CancellationToken ct = default);
    Task<string?> GetFileContentAsync(string owner, string repo, string branch, string path, CancellationToken ct = default);
    Task UpsertFileAsync(string owner, string repo, string branch, string path, string content, string commitMessage, string? existingFileSha, CancellationToken ct = default);
    Task<int> OpenPullRequestAsync(string owner, string repo, string headBranch, string baseBranch, string title, string body, CancellationToken ct = default);
    Task<PullRequestStatus> GetPullRequestStatusAsync(string owner, string repo, int prNumber, CancellationToken ct = default);
    Task<string> MergePullRequestAsync(string owner, string repo, int prNumber, MergeStrategy strategy, CancellationToken ct = default);
    Task ConfigureBranchProtectionAsync(string owner, string repo, string branch, BranchProtectionRules rules, CancellationToken ct = default);
}
