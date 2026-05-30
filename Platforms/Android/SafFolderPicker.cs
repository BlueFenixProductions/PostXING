using Android.Content;
using Microsoft.Maui.ApplicationModel;
using PostXING.ViewModels;
using AndroidUri = Android.Net.Uri;

namespace PostXING.App.Platforms.Android;

/// <summary>SAF folder picker. Fires <c>Intent.ActionOpenDocumentTree</c> via the current
/// MAUI activity and resolves the awaited Task from <see cref="MainActivity.OnActivityResult"/>
/// (the result hook routes back into <see cref="DeliverResult"/>). On success we call
/// <c>TakePersistableUriPermission</c> so the grant survives app restarts.</summary>
public sealed class SafFolderPicker : IFolderPicker
{
    // Request code is a 16-bit magic number that distinguishes our SAF intent from any future
    // startActivityForResult flows; MainActivity.OnActivityResult only forwards results that match.
    internal const int OpenDocumentTreeRequestCode = 0x50584F44;   // "PXOD"

    private static TaskCompletionSource<AndroidUri?>? s_pending;
    private static readonly Lock s_lock = new();

    public Task<string?> PickFolderAsync(CancellationToken ct = default)
    {
        var activity = Platform.CurrentActivity
            ?? throw new InvalidOperationException("No current activity to host the folder picker.");

        TaskCompletionSource<AndroidUri?> tcs;
        lock (s_lock)
        {
            // A second concurrent pick would clobber the first's continuation; surface it instead.
            if (s_pending is not null)
                throw new InvalidOperationException("A folder pick is already in progress.");
            tcs = new TaskCompletionSource<AndroidUri?>(TaskCreationOptions.RunContinuationsAsynchronously);
            s_pending = tcs;
        }

        ct.Register(() =>
        {
            lock (s_lock)
            {
                if (s_pending == tcs) s_pending = null;
            }
            tcs.TrySetCanceled(ct);
        });

        var intent = new Intent(Intent.ActionOpenDocumentTree);
        // Pre-grant the flags we'll persist after the user picks. Without these the persistable
        // permission take in DeliverResult is a no-op.
        intent.AddFlags(ActivityFlags.GrantReadUriPermission
                      | ActivityFlags.GrantWriteUriPermission
                      | ActivityFlags.GrantPersistableUriPermission);

        activity.StartActivityForResult(intent, OpenDocumentTreeRequestCode);

        return ContinueAsync(tcs.Task, activity.ContentResolver);

        static async Task<string?> ContinueAsync(Task<AndroidUri?> task, ContentResolver? resolver)
        {
            var uri = await task.ConfigureAwait(false);
            if (uri is null || resolver is null) return null;
            // Persist the grant so the URI is usable across process death + reboots. The system
            // caps the number of persisted grants per app; we're only ever holding one.
            resolver.TakePersistableUriPermission(uri,
                ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
            return uri.ToString();
        }
    }

    /// <summary>Called by <see cref="MainActivity.OnActivityResult"/> when the SAF picker returns.
    /// <paramref name="data"/> is null on cancel.</summary>
    internal static void DeliverResult(global::Android.App.Result resultCode, Intent? data)
    {
        TaskCompletionSource<AndroidUri?>? tcs;
        lock (s_lock)
        {
            tcs = s_pending;
            s_pending = null;
        }
        if (tcs is null) return;

        if (resultCode == global::Android.App.Result.Ok && data?.Data is { } uri)
            tcs.TrySetResult(uri);
        else
            tcs.TrySetResult(null);
    }
}
