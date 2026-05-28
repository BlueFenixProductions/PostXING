namespace PostXING.Core.Domain;

public enum PostSource
{
    New,
    LocalFile,
    GitHub,
}

public sealed record PostHandle(PostSource Source, string Identifier, string DisplayName)
{
    public static PostHandle NewDraft { get; } = new(PostSource.New, string.Empty, "(new)");

    public static PostHandle FromLocalPath(string fullPath) =>
        new(PostSource.LocalFile, fullPath, System.IO.Path.GetFileName(fullPath));

    public static PostHandle FromGitHubPath(string repoRelativePath) =>
        new(PostSource.GitHub, repoRelativePath, repoRelativePath);
}
