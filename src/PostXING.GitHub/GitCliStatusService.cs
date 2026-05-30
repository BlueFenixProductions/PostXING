using System.Diagnostics;
using System.Globalization;

namespace PostXING.GitHub;

/// <summary>Runs a `git` invocation and returns its exit code and captured output.</summary>
public delegate Task<(int exit, string stdout, string stderr)> GitRunner(IReadOnlyList<string> args, CancellationToken ct);

public sealed class GitCliStatusService : IGitStatusService
{
    private readonly GitRunner _run;

    public GitCliStatusService(string gitPath = "git") : this(MakeProcessRunner(gitPath)) { }

    // Test seam: inject a fake runner to exercise the parsing/mapping without a real repo.
    public GitCliStatusService(GitRunner runner) => _run = runner;

    public async Task<RepoSyncStatus> GetStatusAsync(string? folder, bool fetch, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            return RepoSyncStatus.Unknown("no folder set");

        try
        {
            var inside = await _run(["-C", folder, "rev-parse", "--is-inside-work-tree"], ct);
            if (inside.exit != 0 || inside.stdout.Trim() != "true")
                return RepoSyncStatus.Unknown("not a git repo");

            if (fetch)
                await _run(["-C", folder, "fetch", "--quiet"], ct);   // best-effort; ignore offline/unauth failures

            var status = await _run(["-C", folder, "status", "--porcelain"], ct);
            var dirty = status.exit == 0 ? CountNonBlankLines(status.stdout) : 0;

            // left-right count over @{u}...HEAD: left = upstream-only (behind), right = HEAD-only (ahead).
            var counts = await _run(["-C", folder, "rev-list", "--left-right", "--count", "@{u}...HEAD"], ct);
            if (counts.exit != 0)
                return new RepoSyncStatus(RepoSyncState.Unknown, 0, 0, dirty, "no upstream branch");

            var (behind, ahead) = ParseLeftRight(counts.stdout);
            var state = RepoSyncStatus.Evaluate(insideWorkTree: true, hasUpstream: true, dirty, ahead, behind);
            return new RepoSyncStatus(state, ahead, behind, dirty, Describe(state, ahead, behind, dirty));
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return RepoSyncStatus.Unknown("git not found");
        }
    }

    public async Task<GitOperationResult> CommitAsync(string? folder, string message, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            return new GitOperationResult(false, "no folder set");
        if (string.IsNullOrWhiteSpace(message))
            return new GitOperationResult(false, "commit message required");

        try
        {
            var addRes = await _run(["-C", folder, "add", "-A"], ct);
            if (addRes.exit != 0)
                return new GitOperationResult(false, $"add failed: {Trim(addRes.stderr, addRes.stdout)}");

            var commitRes = await _run(["-C", folder, "commit", "-m", message], ct);
            if (commitRes.exit == 0)
                return new GitOperationResult(true, "committed");

            // `git commit` exits non-zero on "nothing to commit" — treat that as benign success.
            var combined = (commitRes.stdout + "\n" + commitRes.stderr);
            if (combined.Contains("nothing to commit", StringComparison.OrdinalIgnoreCase))
                return new GitOperationResult(true, "nothing to commit");
            return new GitOperationResult(false, $"commit failed: {Trim(commitRes.stderr, commitRes.stdout)}");
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return new GitOperationResult(false, "git not found");
        }
    }

    public async Task<GitOperationResult> PushAsync(string? folder, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            return new GitOperationResult(false, "no folder set");
        try
        {
            var res = await _run(["-C", folder, "push"], ct);
            return res.exit == 0
                ? new GitOperationResult(true, "pushed")
                : new GitOperationResult(false, $"push failed: {Trim(res.stderr, res.stdout)}");
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return new GitOperationResult(false, "git not found");
        }
    }

    public async Task<GitOperationResult> PullAsync(string? folder, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            return new GitOperationResult(false, "no folder set");
        try
        {
            // --ff-only refuses to silently merge a diverged history; the user is asked to
            // resolve that in their git client (we don't drive interactive rebases from here).
            var res = await _run(["-C", folder, "pull", "--ff-only"], ct);
            return res.exit == 0
                ? new GitOperationResult(true, "pulled")
                : new GitOperationResult(false, $"pull failed: {Trim(res.stderr, res.stdout)}");
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return new GitOperationResult(false, "git not found");
        }
    }

    // git outputs to stderr or stdout depending on the command + git version; pick the non-empty
    // one (preferring stderr) and collapse whitespace runs to keep the status-line summary tidy.
    private static string Trim(string stderr, string stdout)
    {
        var s = (string.IsNullOrWhiteSpace(stderr) ? stdout : stderr).Trim();
        return s.Length == 0 ? "(no output)" : s.Replace("\r", "").Replace('\n', ' ');
    }

    private static int CountNonBlankLines(string s) =>
        s.Split('\n').Count(l => !string.IsNullOrWhiteSpace(l));

    private static (int behind, int ahead) ParseLeftRight(string s)
    {
        var parts = s.Split(['\t', ' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
        var behind = parts.Length > 0 && int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var b) ? b : 0;
        var ahead = parts.Length > 1 && int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var a) ? a : 0;
        return (behind, ahead);
    }

    private static string Describe(RepoSyncState state, int ahead, int behind, int dirty) => state switch
    {
        RepoSyncState.Synced => "Up to date with the remote.",
        RepoSyncState.NeedsPull => $"{behind} commit(s) to pull from the remote.",
        RepoSyncState.LocalChanges => string.Join(", ", new[]
        {
            dirty > 0 ? $"{dirty} uncommitted change(s)" : null,
            ahead > 0 ? $"{ahead} commit(s) to push" : null,
        }.Where(p => p is not null)) + ".",
        _ => string.Empty,
    };

    private static GitRunner MakeProcessRunner(string gitPath) => async (args, ct) =>
    {
        var psi = new ProcessStartInfo(gitPath)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        // Never block the background fetch on a credential prompt: fail fast instead. A failed
        // fetch is best-effort, so the chip just falls back to the last-fetched remote state.
        psi.Environment["GIT_TERMINAL_PROMPT"] = "0";
        foreach (var a in args) psi.ArgumentList.Add(a);

        using var p = new Process { StartInfo = psi };
        if (!p.Start())
            throw new InvalidOperationException($"Failed to launch `{gitPath}`. Is git installed and on PATH?");

        var stdoutTask = p.StandardOutput.ReadToEndAsync(ct);
        var stderrTask = p.StandardError.ReadToEndAsync(ct);
        await p.WaitForExitAsync(ct);
        return (p.ExitCode, await stdoutTask, await stderrTask);
    };
}
