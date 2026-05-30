using PostXING.GitHub;
using Shouldly;
using Xunit;

namespace PostXING.GitHub.Tests;

public sealed class RepoSyncStateEvaluateTests
{
    [Theory]
    [InlineData(false, true, 0, 0, 0, RepoSyncState.Unknown)]      // not a repo
    [InlineData(true, false, 0, 0, 0, RepoSyncState.Unknown)]      // no upstream
    [InlineData(true, true, 0, 0, 0, RepoSyncState.Synced)]        // clean + level
    [InlineData(true, true, 3, 0, 0, RepoSyncState.LocalChanges)]  // dirty
    [InlineData(true, true, 0, 2, 0, RepoSyncState.LocalChanges)]  // unpushed (ahead)
    [InlineData(true, true, 0, 0, 4, RepoSyncState.NeedsPull)]     // behind
    [InlineData(true, true, 1, 1, 1, RepoSyncState.LocalChanges)]  // diverged + dirty -> local wins
    [InlineData(true, true, 0, 1, 1, RepoSyncState.LocalChanges)]  // diverged -> local (merge) wins over pull
    public void Evaluate_maps_facts_to_state(bool inside, bool upstream, int dirty, int ahead, int behind, RepoSyncState expected)
        => RepoSyncStatus.Evaluate(inside, upstream, dirty, ahead, behind).ShouldBe(expected);
}

public sealed class GitCliStatusServiceTests
{
    // The service dispatches on the git subcommand (args[2]): -C <folder> <subcommand> ...
    private static GitRunner FakeGit(
        string? insideWorkTree = "true",
        string porcelain = "",
        string revList = "0\t0",          // "<behind>\t<ahead>"
        bool upstreamFails = false,
        Action<IReadOnlyList<string>>? record = null,
        Func<IReadOnlyList<string>, (int exit, string stdout, string stderr)?>? overrideRun = null)
        => (args, ct) =>
        {
            record?.Invoke(args);
            if (overrideRun is not null)
            {
                var custom = overrideRun(args);
                if (custom is not null) return Task.FromResult<(int, string, string)>(custom.Value);
            }
            var sub = args.Count > 2 ? args[2] : string.Empty;
            return Task.FromResult<(int, string, string)>(sub switch
            {
                "rev-parse" => insideWorkTree is null ? (1, string.Empty, "fatal: not a git repository") : (0, insideWorkTree + "\n", string.Empty),
                "fetch" => (0, string.Empty, string.Empty),
                "status" => (0, porcelain, string.Empty),
                "rev-list" => upstreamFails ? (128, string.Empty, "fatal: no upstream configured") : (0, revList + "\n", string.Empty),
                _ => (0, string.Empty, string.Empty),
            });
        };

    private static string TempRepo() => Directory.CreateTempSubdirectory("pxsync").FullName;

    [Fact]
    public async Task Null_or_missing_folder_is_unknown()
    {
        var svc = new GitCliStatusService(FakeGit());
        (await svc.GetStatusAsync(null, fetch: false)).State.ShouldBe(RepoSyncState.Unknown);
        (await svc.GetStatusAsync(Path.Combine(Path.GetTempPath(), "px-does-not-exist-xyz"), fetch: false))
            .State.ShouldBe(RepoSyncState.Unknown);
    }

    [Fact]
    public async Task Not_a_git_repo_is_unknown()
    {
        var dir = TempRepo();
        try
        {
            var s = await new GitCliStatusService(FakeGit(insideWorkTree: null)).GetStatusAsync(dir, fetch: false);
            s.State.ShouldBe(RepoSyncState.Unknown);
            s.Detail.ShouldContain("not a git repo");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Clean_and_level_is_synced()
    {
        var dir = TempRepo();
        try
        {
            var s = await new GitCliStatusService(FakeGit(porcelain: "", revList: "0\t0")).GetStatusAsync(dir, fetch: false);
            s.State.ShouldBe(RepoSyncState.Synced);
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Behind_is_needs_pull()
    {
        var dir = TempRepo();
        try
        {
            var s = await new GitCliStatusService(FakeGit(porcelain: "", revList: "3\t0")).GetStatusAsync(dir, fetch: false);
            s.State.ShouldBe(RepoSyncState.NeedsPull);
            s.Behind.ShouldBe(3);
            s.Ahead.ShouldBe(0);
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Dirty_tree_is_local_changes()
    {
        var dir = TempRepo();
        try
        {
            var s = await new GitCliStatusService(FakeGit(porcelain: " M drafts/a.md\n?? drafts/b.md\n", revList: "0\t0"))
                .GetStatusAsync(dir, fetch: false);
            s.State.ShouldBe(RepoSyncState.LocalChanges);
            s.DirtyFiles.ShouldBe(2);
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Unpushed_commits_are_local_changes()
    {
        var dir = TempRepo();
        try
        {
            var s = await new GitCliStatusService(FakeGit(porcelain: "", revList: "0\t2")).GetStatusAsync(dir, fetch: false);
            s.State.ShouldBe(RepoSyncState.LocalChanges);
            s.Ahead.ShouldBe(2);
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task No_upstream_is_unknown()
    {
        var dir = TempRepo();
        try
        {
            var s = await new GitCliStatusService(FakeGit(upstreamFails: true)).GetStatusAsync(dir, fetch: false);
            s.State.ShouldBe(RepoSyncState.Unknown);
            s.Detail.ShouldContain("upstream");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Fetch_true_runs_git_fetch_first()
    {
        var dir = TempRepo();
        try
        {
            var calls = new List<string>();
            var svc = new GitCliStatusService(FakeGit(record: a => calls.Add(a.Count > 2 ? a[2] : string.Empty)));
            await svc.GetStatusAsync(dir, fetch: true);
            calls.ShouldContain("fetch");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Fetch_false_does_not_fetch()
    {
        var dir = TempRepo();
        try
        {
            var calls = new List<string>();
            var svc = new GitCliStatusService(FakeGit(record: a => calls.Add(a.Count > 2 ? a[2] : string.Empty)));
            await svc.GetStatusAsync(dir, fetch: false);
            calls.ShouldNotContain("fetch");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task CommitAsync_runs_add_then_commit_and_returns_success()
    {
        var dir = TempRepo();
        try
        {
            var subs = new List<string>();
            var svc = new GitCliStatusService(FakeGit(record: a => subs.Add(a.Count > 2 ? a[2] : string.Empty)));
            var result = await svc.CommitAsync(dir, "first draft");
            result.Success.ShouldBeTrue();
            // Both subcommands must fire, in order — `add -A` is what stages the untracked drafts.
            subs.ShouldContain("add");
            subs.ShouldContain("commit");
            subs.IndexOf("add").ShouldBeLessThan(subs.IndexOf("commit"));
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task CommitAsync_treats_nothing_to_commit_as_success()
    {
        var dir = TempRepo();
        try
        {
            // `git commit` exits non-zero when there's nothing staged; the service must not
            // surface that as a failure or the sync chip flashes red for a no-op.
            var svc = new GitCliStatusService(FakeGit(overrideRun: a =>
                a.Count > 2 && a[2] == "commit" ? (1, "nothing to commit, working tree clean", string.Empty) : null));
            var result = await svc.CommitAsync(dir, "no changes");
            result.Success.ShouldBeTrue();
            result.Detail.ShouldContain("nothing to commit");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task CommitAsync_surfaces_real_failure()
    {
        var dir = TempRepo();
        try
        {
            var svc = new GitCliStatusService(FakeGit(overrideRun: a =>
                a.Count > 2 && a[2] == "commit" ? (1, string.Empty, "fatal: empty ident name") : null));
            var result = await svc.CommitAsync(dir, "msg");
            result.Success.ShouldBeFalse();
            result.Detail.ShouldContain("empty ident");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task PushAsync_runs_git_push_and_returns_success()
    {
        var dir = TempRepo();
        try
        {
            var subs = new List<string>();
            var svc = new GitCliStatusService(FakeGit(record: a => subs.Add(a.Count > 2 ? a[2] : string.Empty)));
            (await svc.PushAsync(dir)).Success.ShouldBeTrue();
            subs.ShouldContain("push");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task PushAsync_surfaces_failure()
    {
        var dir = TempRepo();
        try
        {
            var svc = new GitCliStatusService(FakeGit(overrideRun: a =>
                a.Count > 2 && a[2] == "push" ? (1, string.Empty, "fatal: Authentication failed") : null));
            var result = await svc.PushAsync(dir);
            result.Success.ShouldBeFalse();
            result.Detail.ShouldContain("Authentication");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task PullAsync_runs_ff_only_pull_and_returns_success()
    {
        var dir = TempRepo();
        try
        {
            var captured = new List<IReadOnlyList<string>>();
            var svc = new GitCliStatusService(FakeGit(record: a => captured.Add(a)));
            (await svc.PullAsync(dir)).Success.ShouldBeTrue();
            // --ff-only is load-bearing: it refuses a silent auto-merge if the local has diverged.
            captured.ShouldContain(a => a.Count >= 4 && a[2] == "pull" && a.Contains("--ff-only"));
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task PullAsync_reports_diverged_history_failure()
    {
        var dir = TempRepo();
        try
        {
            var svc = new GitCliStatusService(FakeGit(overrideRun: a =>
                a.Count > 2 && a[2] == "pull" ? (128, string.Empty, "fatal: Not possible to fast-forward, aborting.") : null));
            var result = await svc.PullAsync(dir);
            result.Success.ShouldBeFalse();
            result.Detail.ShouldContain("fast-forward");
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task Mutations_with_no_folder_are_failure()
    {
        var svc = new GitCliStatusService(FakeGit());
        (await svc.CommitAsync(null, "msg")).Success.ShouldBeFalse();
        (await svc.PushAsync(null)).Success.ShouldBeFalse();
        (await svc.PullAsync(null)).Success.ShouldBeFalse();
    }
}
