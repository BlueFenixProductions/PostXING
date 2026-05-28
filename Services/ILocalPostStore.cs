namespace PostXING.App.Services;

public sealed record LocalPostFile(string FullPath, string RelativePath);

public interface ILocalPostStore
{
    IReadOnlyList<LocalPostFile> List(string folder);
    Task<string?> ReadAsync(string fullPath, CancellationToken ct = default);
    Task WriteAsync(string fullPath, string contents, CancellationToken ct = default);
}

public sealed class FileSystemLocalPostStore : ILocalPostStore
{
    public IReadOnlyList<LocalPostFile> List(string folder)
    {
        if (!Directory.Exists(folder)) return [];
        return Directory.EnumerateFiles(folder, "*.md", SearchOption.AllDirectories)
            .Select(p => new LocalPostFile(p, Path.GetRelativePath(folder, p).Replace('\\', '/')))
            .OrderBy(f => f.RelativePath, StringComparer.Ordinal)
            .ToList();
    }

    public async Task<string?> ReadAsync(string fullPath, CancellationToken ct = default)
    {
        if (!File.Exists(fullPath)) return null;
        return await File.ReadAllTextAsync(fullPath, ct);
    }

    public async Task WriteAsync(string fullPath, string contents, CancellationToken ct = default)
    {
        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(fullPath, contents, ct);
    }
}
