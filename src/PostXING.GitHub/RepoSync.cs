namespace PostXING.GitHub;

/// <summary>
/// Sync state of a local working copy relative to its GitHub remote, for the status-bar chip.
/// Colors (set in the UI): Synced = green, NeedsPull = orange, LocalChanges = blue, Unknown = neutral.
/// </summary>
public enum RepoSyncState
{
    /// <summary>No folder set, not a git repo, no tracking branch, or git unavailable.</summary>
    Unknown,
    /// <summary>Clean working tree, level with the upstream branch.</summary>
    Synced,
    /// <summary>Upstream has commits the local branch doesn't (behind &gt; 0).</summary>
    NeedsPull,
    /// <summary>Uncommitted changes, or local commits not yet pushed/merged (dirty or ahead &gt; 0).</summary>
    LocalChanges,
}

/// <summary>Sync facts for the working copy plus the derived <see cref="RepoSyncState"/>.</summary>
public sealed record RepoSyncStatus(RepoSyncState State, int Ahead, int Behind, int DirtyFiles, string Detail)
{
    public static RepoSyncStatus Unknown(string detail) => new(RepoSyncState.Unknown, 0, 0, 0, detail);

    /// <summary>
    /// Pure mapping from raw git facts to a sync state. Local work (a dirty tree or unpushed
    /// commits) wins over a pending pull, because "commit/merge first" is the user's next action.
    /// </summary>
    public static RepoSyncState Evaluate(bool insideWorkTree, bool hasUpstream, int dirtyFiles, int ahead, int behind)
    {
        if (!insideWorkTree || !hasUpstream) return RepoSyncState.Unknown;
        if (dirtyFiles > 0 || ahead > 0) return RepoSyncState.LocalChanges;
        if (behind > 0) return RepoSyncState.NeedsPull;
        return RepoSyncState.Synced;
    }
}
