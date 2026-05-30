using System.Text;
using Android.Content;
using Android.Provider;
using Microsoft.Maui.ApplicationModel;
using PostXING.ViewModels;
using AndroidUri = Android.Net.Uri;

namespace PostXING.App.Platforms.Android;

/// <summary>SAF-backed implementation of <see cref="ILocalPostStore"/>. <c>folder</c> is a
/// persisted tree URI string (from <see cref="SafFolderPicker"/>); the per-file <c>Id</c> we
/// hand back is a document URI string. We only look at two hard-coded subfolders (drafts,
/// posts) per the flat-folder convention; nested years/months are not supported and the
/// store will not enumerate them.</summary>
public sealed class SafLocalPostStore : ILocalPostStore
{
    private static readonly string[] TopLevelDirs = ["drafts", "posts"];

    // Common projections — keep ordering stable so the column indices below stay valid.
    private static readonly string[] ChildProjection =
    [
        DocumentsContract.Document.ColumnDocumentId,
        DocumentsContract.Document.ColumnDisplayName,
        DocumentsContract.Document.ColumnMimeType,
        DocumentsContract.Document.ColumnLastModified,
    ];

    private static ContentResolver Resolver =>
        Platform.AppContext.ContentResolver
        ?? throw new InvalidOperationException("No ContentResolver available.");

    public IReadOnlyList<LocalPostFile> List(string folder)
    {
        if (string.IsNullOrEmpty(folder)) return [];

        AndroidUri treeUri;
        try { treeUri = AndroidUri.Parse(folder) ?? throw new ArgumentException(folder); }
        catch { return []; }

        try
        {
            var rootDocId = DocumentsContract.GetTreeDocumentId(treeUri);
            if (rootDocId is null) return [];
            var results = new List<LocalPostFile>();
            foreach (var subdirName in TopLevelDirs)
            {
                var subdir = FindChild(treeUri, rootDocId, subdirName, requireDir: true);
                if (subdir is null) continue;
                EnumerateMarkdownChildren(treeUri, subdir.Value.DocumentId, subdirName, results);
            }
            return results
                .OrderByDescending(f => f.LastWriteTimeUtc)
                .ToList();
        }
        catch
        {
            // SecurityException (revoked grant), provider-gone, etc. — treat as "no files".
            // The Settings page lets the user re-pick the folder.
            return [];
        }
    }

    public async Task<string?> ReadAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(id)) return null;
        var uri = AndroidUri.Parse(id);
        if (uri is null) return null;

        return await Task.Run(() =>
        {
            try
            {
                using var stream = Resolver.OpenInputStream(uri);
                if (stream is null) return null;
                using var reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch { return null; }
        }, ct).ConfigureAwait(false);
    }

    public async Task WriteAsync(string id, string contents, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentException("Empty document id.", nameof(id));
        var uri = AndroidUri.Parse(id) ?? throw new ArgumentException("Not a content URI.", nameof(id));
        var bytes = Encoding.UTF8.GetBytes(contents);

        await Task.Run(() =>
        {
            // "wt" = write+truncate. Plain "w" leaves the prior tail behind on some providers,
            // which would silently corrupt a shrinking edit.
            using var stream = Resolver.OpenOutputStream(uri, "wt")
                ?? throw new IOException("Provider returned no output stream.");
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }, ct).ConfigureAwait(false);
    }

    public async Task<string> CreateAsync(string folder, string relativePath, string contents, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(folder)) throw new ArgumentException("Empty tree URI.", nameof(folder));
        if (string.IsNullOrEmpty(relativePath)) throw new ArgumentException("Empty relative path.", nameof(relativePath));

        var treeUri = AndroidUri.Parse(folder) ?? throw new ArgumentException("Not a content URI.", nameof(folder));
        var parts = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 1) throw new ArgumentException("Path has no filename.", nameof(relativePath));
        var fileName = parts[^1];
        var dirSegments = parts[..^1];

        return await Task.Run(() =>
        {
            var parentDocId = DocumentsContract.GetTreeDocumentId(treeUri)
                ?? throw new InvalidOperationException("Tree URI has no tree document id.");
            foreach (var seg in dirSegments)
            {
                var existing = FindChild(treeUri, parentDocId, seg, requireDir: true);
                parentDocId = existing?.DocumentId ?? CreateChildDirectory(treeUri, parentDocId, seg);
            }

            var parentDocUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, parentDocId);
            var newDocUri = DocumentsContract.CreateDocument(Resolver, parentDocUri!, "text/markdown", fileName)
                ?? throw new IOException($"CreateDocument returned null for {relativePath}.");

            var bytes = Encoding.UTF8.GetBytes(contents);
            using (var stream = Resolver.OpenOutputStream(newDocUri, "wt")
                ?? throw new IOException("Provider returned no output stream for the new document."))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            return newDocUri.ToString()
                ?? throw new IOException("Created document URI rendered as null.");
        }, ct).ConfigureAwait(false);
    }

    private static string CreateChildDirectory(AndroidUri treeUri, string parentDocId, string name)
    {
        var parentDocUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, parentDocId);
        var dirUri = DocumentsContract.CreateDocument(Resolver, parentDocUri!, DocumentsContract.Document.MimeTypeDir, name)
            ?? throw new IOException($"CreateDocument returned null for directory '{name}'.");
        return DocumentsContract.GetDocumentId(dirUri)
            ?? throw new IOException($"Created directory '{name}' has no document id.");
    }

    private readonly record struct ChildRow(string DocumentId, string DisplayName, string? MimeType, long LastModifiedMs);

    private static ChildRow? FindChild(AndroidUri treeUri, string parentDocId, string name, bool requireDir)
    {
        var childrenUri = DocumentsContract.BuildChildDocumentsUriUsingTree(treeUri, parentDocId);
        using var cursor = Resolver.Query(childrenUri!, ChildProjection, null, null, null);
        if (cursor is null) return null;
        while (cursor.MoveToNext())
        {
            var displayName = cursor.GetString(1) ?? string.Empty;
            if (!string.Equals(displayName, name, StringComparison.Ordinal)) continue;
            var mime = cursor.GetString(2);
            if (requireDir && mime != DocumentsContract.Document.MimeTypeDir) continue;
            return new ChildRow(
                DocumentId: cursor.GetString(0) ?? string.Empty,
                DisplayName: displayName,
                MimeType: mime,
                LastModifiedMs: cursor.GetLong(3));
        }
        return null;
    }

    private static void EnumerateMarkdownChildren(AndroidUri treeUri, string parentDocId, string subdirName, List<LocalPostFile> sink)
    {
        var childrenUri = DocumentsContract.BuildChildDocumentsUriUsingTree(treeUri, parentDocId);
        using var cursor = Resolver.Query(childrenUri!, ChildProjection, null, null, null);
        if (cursor is null) return;
        while (cursor.MoveToNext())
        {
            var mime = cursor.GetString(2);
            if (mime == DocumentsContract.Document.MimeTypeDir) continue;
            var displayName = cursor.GetString(1) ?? string.Empty;
            if (!displayName.EndsWith(".md", StringComparison.OrdinalIgnoreCase)) continue;
            var docId = cursor.GetString(0) ?? string.Empty;
            var docUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, docId);
            var lastMs = cursor.GetLong(3);
            sink.Add(new LocalPostFile(
                Id: docUri!.ToString()!,
                RelativePath: $"{subdirName}/{displayName}",
                LastWriteTimeUtc: DateTimeOffset.FromUnixTimeMilliseconds(lastMs)));
        }
    }
}
