using PostXING.ViewModels;

namespace PostXING.App.Services;

/// <summary>Desktop placeholder — there's no folder picker on Windows yet, the user types
/// the path into the Settings Entry. Wired so DI for <see cref="IFolderPicker"/> resolves on
/// every TFM; the "Pick folder" button is hidden via <c>OnPlatform</c> on Windows so this is
/// never invoked.</summary>
public sealed class NoOpFolderPicker : IFolderPicker
{
    public Task<string?> PickFolderAsync(CancellationToken ct = default) => Task.FromResult<string?>(null);
}
