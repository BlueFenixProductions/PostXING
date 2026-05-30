namespace PostXING.GitHub;

/// <summary>
/// Reads the git sync state of a local working copy. Shell-out to the `git` CLI, mirroring the
/// `gh` gateway's approach (no embedded git library, per the project's "shell-out only" rule).
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
}
