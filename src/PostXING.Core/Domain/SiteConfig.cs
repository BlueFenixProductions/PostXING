namespace PostXING.Core.Domain;

public sealed record SiteConfig
{
    public string Owner { get; init; }
    public string Repo { get; init; }
    public string DevelopBranch { get; init; }
    public string StageBranch { get; init; }
    public string MainBranch { get; init; }

    public SiteConfig(
        string owner,
        string repo,
        string developBranch,
        string stageBranch,
        string mainBranch)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        ArgumentException.ThrowIfNullOrWhiteSpace(repo);
        ArgumentException.ThrowIfNullOrWhiteSpace(developBranch);
        ArgumentException.ThrowIfNullOrWhiteSpace(stageBranch);
        ArgumentException.ThrowIfNullOrWhiteSpace(mainBranch);
        Owner = owner;
        Repo = repo;
        DevelopBranch = developBranch;
        StageBranch = stageBranch;
        MainBranch = mainBranch;
    }

    public static SiteConfig For(string owner, string repo) =>
        new(owner, repo, "develop", "stage", "main");
}
