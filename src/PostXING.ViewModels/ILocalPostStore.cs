namespace PostXING.ViewModels;

/// <summary>A post file in the local store. <see cref="Id"/> is the opaque store identifier
/// (a filesystem path on desktop, a SAF document URI on Android) passed back to
/// <see cref="ILocalPostStore.ReadAsync"/>/<see cref="ILocalPostStore.WriteAsync"/>;
/// <see cref="RelativePath"/> is the forward-slash path under the folder (e.g. "drafts/foo.md").</summary>
public sealed record LocalPostFile(string Id, string RelativePath, DateTimeOffset LastWriteTimeUtc);

public interface ILocalPostStore
{
    IReadOnlyList<LocalPostFile> List(string folder);

    Task<string?> ReadAsync(string id, CancellationToken ct = default);

    Task WriteAsync(string id, string contents, CancellationToken ct = default);

    /// <summary>Create a new file under <paramref name="folder"/> at the forward-slash relative
    /// path (e.g. "drafts/foo.md"), write <paramref name="contents"/>, and return its opaque store
    /// identifier. Keeping the path/URI construction here (not in the view models) is what lets the
    /// Android SAF store map "drafts/foo.md" onto a document tree without leaking content:// URIs.</summary>
    Task<string> CreateAsync(string folder, string relativePath, string contents, CancellationToken ct = default);

    /// <summary>Delete the file identified by <paramref name="id"/> (a filesystem path on desktop,
    /// a SAF document URI on Android). A file that's already gone is not an error.</summary>
    Task DeleteAsync(string id, CancellationToken ct = default);
}
