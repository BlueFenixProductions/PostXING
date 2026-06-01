namespace PostXING.GitHub;

/// <summary>
/// Stores the GitHub access token used by the in-process HTTP gateway on platforms without a
/// <c>gh</c> CLI to inherit a credential from (Android). The MAUI app backs this with
/// <c>SecureStorage</c>; tests use <see cref="InMemoryGitHubTokenStore"/>. Read lazily per
/// request so a token set after construction (e.g. just after the user authenticates) is picked
/// up without rebuilding the gateway.
/// </summary>
public interface IGitHubTokenStore
{
    /// <summary>The current token, or <c>null</c> if the user hasn't authenticated yet.</summary>
    Task<string?> GetTokenAsync(CancellationToken ct = default);

    /// <summary>Persist <paramref name="token"/> as the active credential.</summary>
    Task SetTokenAsync(string token, CancellationToken ct = default);

    /// <summary>Forget the stored token (sign out).</summary>
    Task ClearAsync(CancellationToken ct = default);
}
