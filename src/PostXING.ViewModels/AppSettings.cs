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
    // Non-empty is enough: the desktop store handles a missing dir by returning an empty list,
    // and a SAF content:// tree URI (Android) wouldn't satisfy Directory.Exists in any case.
    public bool IsLocalConfigured => !string.IsNullOrWhiteSpace(LocalFolder);
}
