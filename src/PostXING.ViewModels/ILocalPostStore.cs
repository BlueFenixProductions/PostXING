namespace PostXING.ViewModels;

public sealed record LocalPostFile(string FullPath, string RelativePath);

public interface ILocalPostStore
{
    IReadOnlyList<LocalPostFile> List(string folder);
    Task<string?> ReadAsync(string fullPath, CancellationToken ct = default);
    Task WriteAsync(string fullPath, string contents, CancellationToken ct = default);
}
