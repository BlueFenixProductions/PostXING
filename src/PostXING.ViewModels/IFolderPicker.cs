namespace PostXING.ViewModels;

/// <summary>Lets a view model ask the platform to pick a folder. On Android this returns a
/// persisted SAF tree URI string (and the platform impl is responsible for taking the
/// persistable permission); on desktop platforms there is no picker yet, so the implementation
/// returns null and the user types the path into the Entry.</summary>
public interface IFolderPicker
{
    Task<string?> PickFolderAsync(CancellationToken ct = default);
}
