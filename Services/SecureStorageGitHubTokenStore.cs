using Microsoft.Maui.Storage;
using PostXING.GitHub;

namespace PostXING.App.Services;

/// <summary>Platform-backed <see cref="IGitHubTokenStore"/> using MAUI <c>SecureStorage</c>
/// (Android Keystore-backed). Registered on Android, where the in-process HTTP gateway needs a
/// stored credential because there's no <c>gh</c> CLI to inherit one from.</summary>
public sealed class SecureStorageGitHubTokenStore : IGitHubTokenStore
{
    private const string Key = "github_access_token";

    public Task<string?> GetTokenAsync(CancellationToken ct = default) => SecureStorage.Default.GetAsync(Key);

    public Task SetTokenAsync(string token, CancellationToken ct = default) => SecureStorage.Default.SetAsync(Key, token);

    public Task ClearAsync(CancellationToken ct = default)
    {
        _ = SecureStorage.Default.Remove(Key);
        return Task.CompletedTask;
    }
}
