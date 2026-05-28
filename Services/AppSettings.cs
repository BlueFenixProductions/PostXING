namespace PostXING.App.Services;

public sealed record AppSettings(
    string? Owner,
    string? Repo,
    string ContentRoot,
    string DevelopBranch)
{
    public static AppSettings Default { get; } = new(
        Owner: null,
        Repo: null,
        ContentRoot: "content/posts",
        DevelopBranch: "develop");

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Owner) && !string.IsNullOrWhiteSpace(Repo);
}
