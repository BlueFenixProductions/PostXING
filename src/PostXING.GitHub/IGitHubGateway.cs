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
    /// <summary>Returns the GitHub blob sha for the file at <paramref name="path"/> on
    /// <paramref name="branch"/>, or <c>null</c> if the file doesn't exist. Needed by the
    /// Contents API to overwrite an existing file (PUT without a sha is rejected for
    /// existing paths).</summary>
    Task<string?> GetFileShaAsync(string owner, string repo, string branch, string path, CancellationToken ct = default);
    Task UpsertFileAsync(string owner, string repo, string branch, string path, string content, string commitMessage, string? existingFileSha, CancellationToken ct = default);
    /// <summary>Delete the file at <paramref name="path"/> on <paramref name="branch"/> in a single
    /// commit. The Contents API requires the current blob sha (from <see cref="GetFileShaAsync"/>).</summary>
    Task DeleteFileAsync(string owner, string repo, string branch, string path, string commitMessage, string fileSha, CancellationToken ct = default);
    Task<int> OpenPullRequestAsync(string owner, string repo, string headBranch, string baseBranch, string title, string body, CancellationToken ct = default);
    Task<PullRequestStatus> GetPullRequestStatusAsync(string owner, string repo, int prNumber, CancellationToken ct = default);
    Task<string> MergePullRequestAsync(string owner, string repo, int prNumber, MergeStrategy strategy, CancellationToken ct = default);
    Task ConfigureBranchProtectionAsync(string owner, string repo, string branch, BranchProtectionRules rules, CancellationToken ct = default);
    Task<IReadOnlyList<string>> ListMarkdownFilesAsync(string owner, string repo, string branch, string pathPrefix, CancellationToken ct = default);
    Task<GhAuthStatus> CheckAuthAsync(CancellationToken ct = default);
}

public sealed record GhAuthStatus(bool IsAuthenticated, string? Username, string Detail);
