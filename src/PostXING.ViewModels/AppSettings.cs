namespace PostXING.ViewModels;

public sealed record AppSettings(
    string? Owner,
    string? Repo,
    string DevelopBranch,
    string? AuthorName,
    string? LocalFolder,
    string? ContentRoot = null)
{
    public static AppSettings Default { get; } = new(
        Owner: null,
        Repo: null,
        DevelopBranch: "develop",
        AuthorName: null,
        LocalFolder: null,
        ContentRoot: null);

    public bool IsGitHubConfigured => !string.IsNullOrWhiteSpace(Owner) && !string.IsNullOrWhiteSpace(Repo);
    // Non-empty is enough: the desktop store handles a missing dir by returning an empty list,
    // and a SAF content:// tree URI (Android) wouldn't satisfy Directory.Exists in any case.
    public bool IsLocalConfigured => !string.IsNullOrWhiteSpace(LocalFolder);

    // The repo subfolder that holds the drafts/ + posts/ trees (e.g. "blog" -> blog/posts/);
    // empty/null = repo root. The drafts/posts/ names and {date}-{slug} convention are unchanged.
    public string PostsPrefix => Prefix("posts/");
    public string DraftsPrefix => Prefix("drafts/");

    private string Prefix(string folder) =>
        string.IsNullOrWhiteSpace(ContentRoot) ? folder : $"{ContentRoot.Trim().Trim('/')}/{folder}";
}
