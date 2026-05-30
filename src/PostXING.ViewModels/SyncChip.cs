using PostXING.GitHub;

namespace PostXING.ViewModels;

/// <summary>
/// Shared presentation of a <see cref="RepoSyncStatus"/> as the status-bar chip's short label.
/// Used by both the editor and the home screen so the wording stays in one place.
/// </summary>
internal static class SyncChip
{
    public static string Label(RepoSyncStatus status) => status.State switch
    {
        RepoSyncState.Synced => "synced",
        RepoSyncState.NeedsPull => status.Behind > 0 ? $"needs pull ({status.Behind})" : "needs pull",
        RepoSyncState.LocalChanges => "local changes",
        _ => status.Detail,
    };
}
