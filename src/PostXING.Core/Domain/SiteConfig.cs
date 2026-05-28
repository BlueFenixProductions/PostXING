namespace PostXING.Core.Domain;

public enum SiteFlavor
{
    Hugo,
    Jekyll,
    Zola,
    GenericMarkdown,
}

public sealed record SiteConfig
{
    public string Owner { get; init; }
    public string Repo { get; init; }
    public string ContentRoot { get; init; }
    public string DevelopBranch { get; init; }
    public string StageBranch { get; init; }
    public string MainBranch { get; init; }
    public SiteFlavor Flavor { get; init; }

    public SiteConfig(
        string owner,
        string repo,
        string contentRoot,
        string developBranch,
        string stageBranch,
        string mainBranch,
        SiteFlavor flavor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        ArgumentException.ThrowIfNullOrWhiteSpace(repo);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(developBranch);
        ArgumentException.ThrowIfNullOrWhiteSpace(stageBranch);
        ArgumentException.ThrowIfNullOrWhiteSpace(mainBranch);
        Owner = owner;
        Repo = repo;
        ContentRoot = contentRoot;
        DevelopBranch = developBranch;
        StageBranch = stageBranch;
        MainBranch = mainBranch;
        Flavor = flavor;
    }

    public static SiteConfig For(string owner, string repo, string contentRoot, SiteFlavor flavor) =>
        new(owner, repo, contentRoot, "develop", "stage", "main", flavor);
}
