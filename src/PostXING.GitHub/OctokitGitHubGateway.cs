using System.Text;
using Microsoft.Extensions.Logging;
using Octokit;

namespace PostXING.GitHub;

public sealed class OctokitGitHubGateway(IGitHubClient client, ILogger<OctokitGitHubGateway> log) : IGitHubGateway
{
    public async Task<string> GetBranchShaAsync(string owner, string repo, string branch, CancellationToken ct = default)
    {
        var reference = await client.Git.Reference.Get(owner, repo, $"heads/{branch}");
        return reference.Object.Sha;
    }

    public async Task CreateBranchAsync(string owner, string repo, string newBranch, string fromSha, CancellationToken ct = default)
    {
        await client.Git.Reference.Create(owner, repo, new NewReference($"refs/heads/{newBranch}", fromSha));
        log.LogInformation("Created branch {Branch} from {Sha}", newBranch, fromSha);
    }

    public async Task<string?> GetFileContentAsync(string owner, string repo, string branch, string path, CancellationToken ct = default)
    {
        try
        {
            var contents = await client.Repository.Content.GetAllContentsByRef(owner, repo, path, branch);
            if (contents.Count == 0) return null;
            var first = contents[0];
            return first.EncodedContent is null
                ? first.Content
                : Encoding.UTF8.GetString(Convert.FromBase64String(first.EncodedContent));
        }
        catch (NotFoundException)
        {
            return null;
        }
    }

    public async Task UpsertFileAsync(string owner, string repo, string branch, string path, string content, string commitMessage, string? existingFileSha, CancellationToken ct = default)
    {
        if (existingFileSha is null)
        {
            await client.Repository.Content.CreateFile(owner, repo, path,
                new CreateFileRequest(commitMessage, content, branch));
        }
        else
        {
            await client.Repository.Content.UpdateFile(owner, repo, path,
                new UpdateFileRequest(commitMessage, content, existingFileSha, branch));
        }
    }

    public async Task<int> OpenPullRequestAsync(string owner, string repo, string headBranch, string baseBranch, string title, string body, CancellationToken ct = default)
    {
        var pr = await client.PullRequest.Create(owner, repo,
            new NewPullRequest(title, headBranch, baseBranch) { Body = body });
        return pr.Number;
    }

    public async Task<PullRequestStatus> GetPullRequestStatusAsync(string owner, string repo, int prNumber, CancellationToken ct = default)
    {
        var pr = await client.PullRequest.Get(owner, repo, prNumber);
        var headSha = pr.Head.Sha;
        var runs = await client.Check.Run.GetAllForReference(owner, repo, headSha);
        var checks = runs.CheckRuns
            .Select(r => new CheckRunStatus(r.Name, r.Status.ToString() ?? string.Empty, r.Conclusion?.ToString()))
            .ToList();
        return new PullRequestStatus(pr.Number, pr.State.ToString() ?? string.Empty, pr.Mergeable, pr.MergeableState?.ToString(), checks);
    }

    public async Task<string> MergePullRequestAsync(string owner, string repo, int prNumber, MergeStrategy strategy, CancellationToken ct = default)
    {
        var method = strategy switch
        {
            MergeStrategy.Squash => PullRequestMergeMethod.Squash,
            MergeStrategy.Rebase => PullRequestMergeMethod.Rebase,
            _ => PullRequestMergeMethod.Merge,
        };
        var result = await client.PullRequest.Merge(owner, repo, prNumber, new MergePullRequest { MergeMethod = method });
        return result.Sha;
    }

    public Task ConfigureBranchProtectionAsync(string owner, string repo, string branch, BranchProtectionRules rules, CancellationToken ct = default)
    {
        // TODO: Octokit's BranchProtectionSettingsUpdate surface varies across majors;
        // wire this against the actually-installed Octokit version in a follow-up.
        // For now this is intentionally a no-op so the scaffold compiles; use the gh CLI
        // (see scripts/bootstrap-protection.sh in the design doc) to apply protection.
        log.LogWarning("ConfigureBranchProtectionAsync is not implemented; use gh CLI to configure {Owner}/{Repo}#{Branch}", owner, repo, branch);
        return Task.CompletedTask;
    }
}
