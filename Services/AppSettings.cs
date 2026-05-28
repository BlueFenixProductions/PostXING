namespace PostXING.App.Services;

public sealed record AppSettings(
    string? Owner,
    string? Repo,
    string DevelopBranch,
    string? AuthorName)
{
    public static AppSettings Default { get; } = new(
        Owner: null,
        Repo: null,
        DevelopBranch: "develop",
        AuthorName: null);

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Owner) && !string.IsNullOrWhiteSpace(Repo);
}
