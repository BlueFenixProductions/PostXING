using NSubstitute;
using PostXING.Core.Domain;
using PostXING.GitHub;
using PostXING.Markdown;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

public sealed class EditorViewModelTests
{
    // Default git-status stub so constructing the VM (which kicks a sync refresh) is inert.
    private static IGitStatusService StubGit(RepoSyncState state = RepoSyncState.Unknown)
    {
        var git = Substitute.For<IGitStatusService>();
        git.GetStatusAsync(Arg.Any<string?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new RepoSyncStatus(state, 0, 0, 0, "stub"));
        return git;
    }

    /// <summary>
    /// Locks the EditorViewModel ctor signature. PreviewHtml + IMarkdownRenderer were deleted
    /// in the "no dead Markdig render on every keystroke" optimization pass; IGitStatusService was
    /// added for the git sync chip. This test exists so changing the dependency set requires a
    /// deliberate code change accompanied by an updated test.
    /// </summary>
    [Fact]
    public void Ctor_takes_only_the_six_real_dependencies()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(new ParsedDocument(FrontMatter.Default, string.Empty));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var clock = TimeProvider.System;
        var gitStatus = StubGit();

        var vm = new EditorViewModel(parser, gateway, settings, local, clock, gitStatus);

        vm.ShouldNotBeNull();
        vm.ShowTitlePrompt.ShouldBeTrue("New EditorViewModel begins with the title-prompt overlay open.");
        vm.IsDirty.ShouldBeFalse();
        vm.CurrentPath.ShouldBe("(new)");
        vm.RawMarkdown.ShouldBe(string.Empty);
    }

    [Fact]
    public async Task RefreshSync_maps_the_service_result_to_the_chip()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(new ParsedDocument(FrontMatter.Default, string.Empty));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stub"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default with { LocalFolder = @"C:\repo" });
        var local = Substitute.For<ILocalPostStore>();
        var git = Substitute.For<IGitStatusService>();
        git.GetStatusAsync(Arg.Any<string?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new RepoSyncStatus(RepoSyncState.NeedsPull, 0, 2, 0, "2 commit(s) to pull from the remote."));

        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, git);
        await vm.RefreshSyncAsync(fetch: true);

        vm.SyncState.ShouldBe(RepoSyncState.NeedsPull);
        vm.SyncStatus.ShouldBe("needs pull (2)");
        vm.SyncDetail.ShouldContain("pull");
    }

    [Fact]
    public void Typing_into_RawMarkdown_marks_buffer_dirty_and_recalculates_word_count()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse("hello world").Returns(new ParsedDocument(FrontMatter.Default, "hello world"));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit());
        vm.CancelTitlePromptCommand.Execute(null);
        vm.IsDirty = false;

        vm.RawMarkdown = "hello world";

        vm.IsDirty.ShouldBeTrue("Typing into the editor marks the buffer dirty.");
        vm.WordCount.ShouldBe(2);
        vm.ReadingTimeMinutes.ShouldBe(1);
    }

    [Fact]
    public void LoadPost_does_not_mark_dirty()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(new ParsedDocument(FrontMatter.Default, string.Empty));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit());

        vm.LoadPost(PostHandle.FromLocalPath(@"C:\posts\drafts\hello.md"), "# Hello\n");

        vm.IsDirty.ShouldBeFalse("Opening an existing post must not mark the buffer dirty.");
        vm.CurrentPath.ShouldContain("hello.md");
        vm.ShowTitlePrompt.ShouldBeFalse();
    }

    /// <summary>
    /// Regression test: when LoadPost is called, RawMarkdown must update to the loaded
    /// content AND fire PropertyChanged so the EditorPage's WebView bridge can push the
    /// content to the JS editor. If this test fails, the "opened post shows empty editor"
    /// regression has returned.
    /// </summary>
    [Fact]
    public void LoadPost_sets_RawMarkdown_and_fires_PropertyChanged_for_RawMarkdown()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(call =>
            new ParsedDocument(FrontMatter.Default, call.Arg<string>()));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit());

        var raised = new List<string?>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        const string contents =
            "---\ntitle: \"Hello\"\nauthor: \"Chris\"\ndate: 2026-05-29\n---\n\n# Hello\n\nWorld.\n";
        vm.LoadPost(PostHandle.FromLocalPath(@"C:\posts\drafts\hello.md"), contents);

        vm.RawMarkdown.ShouldBe(contents, "LoadPost must overwrite RawMarkdown with the loaded contents.");
        raised.ShouldContain(nameof(EditorViewModel.RawMarkdown),
            "RawMarkdown PropertyChanged must fire so the WebView bridge can push the new content. " +
            "Without this event the editor surface stays empty after opening an existing post.");
        vm.WordCount.ShouldBeGreaterThan(0, "WordCount must reflect the loaded body, not the previous state.");
    }

    [Fact]
    public void Loading_a_post_then_typing_keeps_in_sync()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(call =>
            new ParsedDocument(FrontMatter.Default, call.Arg<string>()));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit());

        vm.LoadPost(PostHandle.FromLocalPath(@"C:\posts\post.md"), "original");
        vm.IsDirty.ShouldBeFalse();

        vm.RawMarkdown = "edited";

        vm.IsDirty.ShouldBeTrue("Editing after a load must mark the buffer dirty.");
        vm.RawMarkdown.ShouldBe("edited");
    }

    [Fact]
    public async Task SaveAsync_on_unconfigured_local_folder_emits_a_helpful_status_and_does_not_throw()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>())
            .Returns(call => new ParsedDocument(FrontMatter.Default.WithTitle("Untitled"), call.Arg<string>()));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit());
        vm.CancelTitlePromptCommand.Execute(null);
        vm.RawMarkdown = "some content";

        await vm.SaveCommand.ExecuteAsync(null);

        vm.SaveStatus.ShouldContain("local folder", Case.Insensitive);
        await local.DidNotReceive().WriteAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Edit-sync contract: SaveAsync must await SyncBeforeSaveAsync BEFORE persisting, and the
    /// text that hook leaves in RawMarkdown is what gets written. On Android the JS-&gt;host bridge
    /// can't live-sync edits, so the EditorPage uses this hook to pull the editor's current text
    /// (via EvaluateJavaScriptAsync getText) before the buffer is saved. If this breaks, Android
    /// saves the seed instead of the user's typing.
    /// </summary>
    [Fact]
    public async Task SaveAsync_awaits_SyncBeforeSaveAsync_and_persists_its_pulled_text()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>())
            .Returns(call => new ParsedDocument(FrontMatter.Default.WithTitle("Pulled Post"), call.Arg<string>()));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default with { LocalFolder = @"C:\repo" });
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit());
        vm.CancelTitlePromptCommand.Execute(null);
        vm.RawMarkdown = "typed-but-not-yet-synced";

        var hookRan = false;
        vm.SyncBeforeSaveAsync = () =>
        {
            hookRan = true;
            vm.RawMarkdown = "pulled-from-editor";   // simulates the host pulling editor text
            return Task.CompletedTask;
        };

        await vm.SaveCommand.ExecuteAsync(null);

        hookRan.ShouldBeTrue("SaveAsync must await SyncBeforeSaveAsync before persisting.");
        await local.Received(1).WriteAsync(
            Arg.Any<string>(), "pulled-from-editor", Arg.Any<CancellationToken>());
    }
}
