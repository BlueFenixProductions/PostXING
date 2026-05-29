using NSubstitute;
using PostXING.Core.Domain;
using PostXING.GitHub;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

/// <summary>
/// The Open page is the Shell root (home screen). Selecting a post, "new", and "settings"
/// each navigate AWAY from this page exactly once per visit. These tests lock in the
/// <c>_navigated</c> latch that guards against a fast double-tap pushing two pages.
/// </summary>
public sealed class OpenPostViewModelTests
{
    private static (OpenPostViewModel vm, IGitHubGateway gateway, ISettingsStore settings, ILocalPostStore local) CreateVm()
    {
        var gateway = Substitute.For<IGitHubGateway>();
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        return (new OpenPostViewModel(gateway, settings, local), gateway, settings, local);
    }

    // Hoisted to static fields so the sort assertions/setups below don't pass constant
    // arrays as arguments (CA1861). Names describe the expected newest-first ordering.
    private static readonly string[] LocalDraftsNewestFirst =
        { "drafts/newest.md", "drafts/mid.md", "drafts/old.md" };
    private static readonly string[] GitHubDraftFiles = { "drafts/wip.md" };
    private static readonly string[] GitHubPostFiles =
        { "posts/2026-01-05-old.md", "posts/2026-05-20-new.md" };
    private static readonly string[] GitHubEntriesNewestFirst =
        { "posts/2026-05-20-new.md", "posts/2026-01-05-old.md", "drafts/wip.md" };

    [Fact]
    public async Task Selecting_a_post_hands_it_off_and_navigates_exactly_once()
    {
        var (vm, _, _, local) = CreateVm();
        var entry = new PostEntry(PostSource.LocalFile, @"C:\posts\drafts\a.md", "a.md", "local");
        local.ReadAsync(entry.Identifier).Returns("# A");
        int editor = 0, opened = 0;
        vm.EditorRequested += (_, _) => editor++;
        vm.PostOpened += (_, _) => opened++;

        // Second invocation simulates a tap that lands while the page is already navigating.
        await vm.SelectCommand.ExecuteAsync(entry);
        await vm.SelectCommand.ExecuteAsync(entry);

        editor.ShouldBe(1, "navigation to the editor must fire exactly once per page visit");
        opened.ShouldBe(1, "the post must be handed to the pending box exactly once");
    }

    [Fact]
    public async Task Double_tapping_two_rows_navigates_to_the_editor_only_once()
    {
        var (vm, _, _, local) = CreateVm();
        var a = new PostEntry(PostSource.LocalFile, @"C:\posts\drafts\a.md", "a.md", "local");
        var b = new PostEntry(PostSource.LocalFile, @"C:\posts\drafts\b.md", "b.md", "local");
        local.ReadAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns("# X");
        int editor = 0;
        vm.EditorRequested += (_, _) => editor++;

        var t1 = vm.SelectCommand.ExecuteAsync(a);
        var t2 = vm.SelectCommand.ExecuteAsync(b);
        await Task.WhenAll(t1, t2);

        editor.ShouldBe(1, "a fast double-tap on two rows must not push the editor twice");
    }

    [Fact]
    public void New_button_navigates_only_once_per_visit()
    {
        var (vm, _, _, _) = CreateVm();
        int editor = 0;
        vm.EditorRequested += (_, _) => editor++;

        vm.NewPostCommand.Execute(null);
        vm.NewPostCommand.Execute(null);

        editor.ShouldBe(1, "the 'new' affordance must navigate exactly once per page visit");
    }

    [Fact]
    public void Settings_button_navigates_only_once_per_visit()
    {
        var (vm, _, _, _) = CreateVm();
        int settingsNav = 0;
        vm.SettingsRequested += (_, _) => settingsNav++;

        vm.OpenSettingsCommand.Execute(null);
        vm.OpenSettingsCommand.Execute(null);

        settingsNav.ShouldBe(1, "settings must be pushed exactly once per page visit");
    }

    [Fact]
    public async Task Local_entries_are_listed_newest_first_by_last_write_time()
    {
        var (vm, _, settings, local) = CreateVm();
        var folder = Directory.CreateTempSubdirectory().FullName;
        try
        {
            settings.Current.Returns(AppSettings.Default with { LocalFolder = folder });
            var baseTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
            // Returned in arbitrary (oldest-first) order; the VM must re-sort newest-first.
            local.List(folder).Returns(new[]
            {
                new LocalPostFile(@"C:\p\drafts\old.md",    "drafts/old.md",    baseTime),
                new LocalPostFile(@"C:\p\drafts\newest.md", "drafts/newest.md", baseTime.AddDays(10)),
                new LocalPostFile(@"C:\p\drafts\mid.md",    "drafts/mid.md",    baseTime.AddDays(5)),
            });

            await vm.RefreshAsync();

            vm.Entries.Select(e => e.DisplayName).ShouldBe(LocalDraftsNewestFirst);
        }
        finally { Directory.Delete(folder, recursive: true); }
    }

    [Fact]
    public async Task GitHub_posts_sort_by_filename_date_and_undated_drafts_sink()
    {
        var (vm, gateway, settings, _) = CreateVm();
        settings.Current.Returns(AppSettings.Default with { Owner = "o", Repo = "r" });
        gateway.ListMarkdownFilesAsync("o", "r", "develop", "drafts/")
            .Returns(GitHubDraftFiles);
        gateway.ListMarkdownFilesAsync("o", "r", "develop", "posts/")
            .Returns(GitHubPostFiles);

        await vm.RefreshAsync();

        vm.Entries.Select(e => e.DisplayName).ShouldBe(GitHubEntriesNewestFirst);
    }

    [Fact]
    public void About_button_navigates_only_once_per_visit()
    {
        var (vm, _, _, _) = CreateVm();
        int aboutNav = 0;
        vm.AboutRequested += (_, _) => aboutNav++;

        vm.AboutCommand.Execute(null);
        vm.AboutCommand.Execute(null);

        aboutNav.ShouldBe(1, "about must be pushed exactly once per page visit");
    }

    [Fact]
    public async Task Re_appearing_on_the_page_re_arms_navigation()
    {
        var (vm, _, _, _) = CreateVm();
        int editor = 0;
        vm.EditorRequested += (_, _) => editor++;

        vm.NewPostCommand.Execute(null);
        vm.NewPostCommand.Execute(null);
        editor.ShouldBe(1);

        // RefreshAsync runs from OnAppearing every time the page (re)appears — e.g. after the
        // editor pops back to home — and must re-arm the latch for the new visit.
        await vm.RefreshAsync();
        vm.NewPostCommand.Execute(null);

        editor.ShouldBe(2, "returning to the Open page must allow navigating again");
    }

    [Fact]
    public async Task A_failed_read_re_arms_navigation_so_the_user_can_retry()
    {
        var (vm, _, _, local) = CreateVm();
        var entry = new PostEntry(PostSource.LocalFile, @"C:\posts\drafts\a.md", "a.md", "local");
        // First read fails (null), second succeeds.
        local.ReadAsync(entry.Identifier).Returns((string?)null, "# A");
        int editor = 0;
        vm.EditorRequested += (_, _) => editor++;

        await vm.SelectCommand.ExecuteAsync(entry);
        editor.ShouldBe(0, "a read that returns null must not navigate");

        await vm.SelectCommand.ExecuteAsync(entry);
        editor.ShouldBe(1, "after a failed read the latch must reset so a retry can navigate");
    }
}
