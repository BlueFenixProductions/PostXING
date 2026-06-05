using PostXING.Core.Domain;
using PostXING.ViewModels;

namespace PostXING.App.Views;

public partial class OpenPostPage : ContentPage
{
    private readonly OpenPostViewModel _vm;
    private readonly IPendingPostBox _box;

    public OpenPostPage(OpenPostViewModel vm, IPendingPostBox box)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _box = box;
        vm.PostOpened += (_, e) => box.Put(e);
        vm.EditorRequested += async (_, _) => await Shell.Current.GoToAsync("editor");
        vm.SettingsRequested += async (_, _) => await Shell.Current.GoToAsync("settings");
        vm.AboutRequested += async (_, _) => await Shell.Current.GoToAsync("about");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _vm.RefreshAsync();
    }

    private void OnEntrySelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is PostEntry entry)
            _ = _vm.SelectCommand.ExecuteAsync(entry);

        // Clear so the same row can be tapped again later if needed.
        if (sender is CollectionView cv)
            cv.SelectedItem = null;
    }

    // Swipe-to-delete on a list row -> confirm (destructive) -> the VM does the actual delete.
    private async void OnDeleteSwipeInvoked(object? sender, EventArgs e)
    {
        if (sender is not SwipeItem item || item.CommandParameter is not PostEntry entry) return;
        var detail = entry.Source == PostSource.GitHub
            ? "This commits a deletion to your GitHub repo. It can't be undone from the app."
            : "This permanently deletes the local file.";
        var ok = await DisplayAlertAsync("Delete post?", $"{entry.DisplayName}\n\n{detail}", "delete", "cancel");
        if (!ok) return;
        await _vm.DeleteCommand.ExecuteAsync(entry);
    }

    // Sync chip action sheet — mirrors EditorPage so the Open page can drive the same set of
    // local-git operations. The chip is Windows-only (hidden on Android via OnPlatform).
    private async void OnSyncChipTapped(object? sender, TappedEventArgs e)
    {
        const string commitPush = "commit & push";
        const string commitOnly = "commit";
        const string push = "push";
        const string pull = "pull";
        const string refresh = "refresh";
        var choice = await DisplayActionSheetAsync("git", "cancel", null, commitPush, commitOnly, push, pull, refresh);
        switch (choice)
        {
            case commitPush:
            {
                var msg = await PromptCommitMessageAsync();
                if (msg is null) return;
                await _vm.CommitAsync(msg);
                await _vm.PushLocalAsync();
                break;
            }
            case commitOnly:
            {
                var msg = await PromptCommitMessageAsync();
                if (msg is null) return;
                await _vm.CommitAsync(msg);
                break;
            }
            case push: await _vm.PushLocalAsync(); break;
            case pull: await _vm.PullLocalAsync(); break;
            case refresh: await _vm.RefreshSyncAsync(fetch: true); break;
        }
    }

    private async Task<string?> PromptCommitMessageAsync()
    {
        var msg = await DisplayPromptAsync("commit", "what changed?",
            accept: "commit", cancel: "cancel",
            initialValue: "WIP draft", maxLength: 240);
        return string.IsNullOrWhiteSpace(msg) ? null : msg;
    }
}
