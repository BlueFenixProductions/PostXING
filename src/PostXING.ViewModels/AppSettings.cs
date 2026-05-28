namespace PostXING.ViewModels;

public sealed record AppSettings(
    string? Owner,
    string? Repo,
    string DevelopBranch,
    string? AuthorName,
    string? LocalFolder)
{
    public static AppSettings Default { get; } = new(
        Owner: null,
        Repo: null,
        DevelopBranch: "develop",
        AuthorName: null,
        LocalFolder: null);

    public bool IsGitHubConfigured => !string.IsNullOrWhiteSpace(Owner) && !string.IsNullOrWhiteSpace(Repo);
    public bool IsLocalConfigured => !string.IsNullOrWhiteSpace(LocalFolder) && System.IO.Directory.Exists(LocalFolder);
}
