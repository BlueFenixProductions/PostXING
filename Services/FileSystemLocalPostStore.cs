using PostXING.ViewModels;

namespace PostXING.App.Services;

public sealed class FileSystemLocalPostStore : ILocalPostStore
{
    public IReadOnlyList<LocalPostFile> List(string folder)
    {
        if (!Directory.Exists(folder)) return [];
        return Directory.EnumerateFiles(folder, "*.md", SearchOption.AllDirectories)
            .Select(p => new LocalPostFile(
                p,
                Path.GetRelativePath(folder, p).Replace('\\', '/'),
                new DateTimeOffset(File.GetLastWriteTimeUtc(p), TimeSpan.Zero)))
            .OrderByDescending(f => f.LastWriteTimeUtc)
            .ToList();
    }

    public async Task<string?> ReadAsync(string id, CancellationToken ct = default)
    {
        if (!File.Exists(id)) return null;
        return await File.ReadAllTextAsync(id, ct);
    }

    public async Task WriteAsync(string id, string contents, CancellationToken ct = default)
    {
        var dir = Path.GetDirectoryName(id);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(id, contents, ct);
    }

    public async Task<string> CreateAsync(string folder, string relativePath, string contents, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(folder, relativePath.Replace('/', Path.DirectorySeparatorChar));
        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(fullPath, contents, ct);
        return fullPath;
    }
}
