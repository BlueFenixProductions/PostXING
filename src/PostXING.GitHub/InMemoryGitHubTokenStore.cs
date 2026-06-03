namespace PostXING.GitHub;

/// <summary>Non-persisted <see cref="IGitHubTokenStore"/> for tests and as a safe default before
/// the platform SecureStorage-backed store is registered.</summary>
public sealed class InMemoryGitHubTokenStore : IGitHubTokenStore
{
    private string? _token;

    public InMemoryGitHubTokenStore(string? token = null) => _token = token;

    public Task<string?> GetTokenAsync(CancellationToken ct = default) => Task.FromResult(_token);

    public Task SetTokenAsync(string token, CancellationToken ct = default)
    {
        _token = token;
        return Task.CompletedTask;
    }

    public Task ClearAsync(CancellationToken ct = default)
    {
        _token = null;
        return Task.CompletedTask;
    }
}
