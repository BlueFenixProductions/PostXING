namespace PostXING.Core.Domain;

public enum PublishKind
{
    DraftLocal,
    BranchPushed,
    PullRequestOpen,
    Merged,
    Failed,
}

public sealed record PublishState(
    PublishKind Kind,
    string? BranchName,
    int? PullRequestNumber,
    string? MergeCommitSha,
    string? FailureReason)
{
    public static PublishState Initial { get; } = new(PublishKind.DraftLocal, null, null, null, null);

    public PublishState Transition(PublishKind to) => to switch
    {
        PublishKind.BranchPushed when BranchName is null =>
            throw new InvalidOperationException("Set branch name via WithBranch first."),
        _ => this with { Kind = to },
    };

    public PublishState WithBranch(string branch) =>
        this with { Kind = PublishKind.BranchPushed, BranchName = branch };

    public PublishState WithPullRequest(int number) =>
        this with { Kind = PublishKind.PullRequestOpen, PullRequestNumber = number };

    public PublishState WithMerged(string sha) =>
        this with { Kind = PublishKind.Merged, MergeCommitSha = sha };

    public PublishState WithFailure(string reason) =>
        this with { Kind = PublishKind.Failed, FailureReason = reason };
}
