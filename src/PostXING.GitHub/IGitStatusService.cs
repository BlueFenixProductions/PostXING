namespace PostXING.GitHub;

/// <summary>Result of a git mutation (commit/push/pull). <see cref="Detail"/> is a short
/// human-readable summary suitable for surfacing in the editor's status line.</summary>
public sealed record GitOperationResult(bool Success, string Detail);

/// <summary>
/// Reads the git sync state of a local working copy and runs the small set of mutations the
/// sync chip exposes (commit/push/pull). Shell-out to the `git` CLI, mirroring the `gh`
/// gateway's approach (no embedded git library, per the project's "shell-out only" rule).
/// </summary>
public interface IGitStatusService
{
    /// <summary>
    /// Computes the sync state of the working copy at <paramref name="folder"/>. When
    /// <paramref name="fetch"/> is true, runs a best-effort `git fetch` first so "needs pull"
    /// reflects the live remote; otherwise reports against the last-fetched remote-tracking ref.
    /// Never throws for the expected failure modes (no folder, not a repo, no upstream, git
    /// missing) — those map to <see cref="RepoSyncState.Unknown"/> with a descriptive detail.
    /// </summary>
    Task<RepoSyncStatus> GetStatusAsync(string? folder, bool fetch, CancellationToken ct = default);

    /// <summary>Stages everything (`git add -A`) and commits with <paramref name="message"/>.
    /// "Nothing to commit" is treated as success.</summary>
    Task<GitOperationResult> CommitAsync(string? folder, string message, CancellationToken ct = default);

    /// <summary>`git push` to the upstream branch.</summary>
    Task<GitOperationResult> PushAsync(string? folder, CancellationToken ct = default);

    /// <summary>`git pull --ff-only` — refuses to auto-merge a diverged history; the user is
    /// expected to resolve that in their git client.</summary>
    Task<GitOperationResult> PullAsync(string? folder, CancellationToken ct = default);
}
