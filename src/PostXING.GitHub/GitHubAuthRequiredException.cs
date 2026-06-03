namespace PostXING.GitHub;

/// <summary>Thrown when a GitHub REST call returns HTTP 401 — the stored token is missing,
/// expired, or revoked. The UI catches this to prompt the user to re-authenticate.</summary>
public sealed class GitHubAuthRequiredException : Exception
{
    public GitHubAuthRequiredException() { }

    public GitHubAuthRequiredException(string message) : base(message) { }

    public GitHubAuthRequiredException(string message, Exception innerException)
        : base(message, innerException) { }
}
