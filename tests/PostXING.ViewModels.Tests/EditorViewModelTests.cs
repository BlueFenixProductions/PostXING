using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
    /// added for the git sync chip; GitHubPublishService was added so Save (GH branch) and
    /// Publish-confirm both route through the service layer. This test exists so changing the
    /// dependency set requires a deliberate code change accompanied by an updated test.
    /// </summary>
    [Fact]
    public void Ctor_takes_only_the_seven_real_dependencies()
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
        var publish = new GitHubPublishService(gateway);

        var vm = new EditorViewModel(parser, gateway, settings, local, clock, gitStatus, publish);

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

        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, git, new GitHubPublishService(gateway));
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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));
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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));

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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));

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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));

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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));
        vm.CancelTitlePromptCommand.Execute(null);
        vm.RawMarkdown = "some content";

        await vm.SaveCommand.ExecuteAsync(null);

        vm.SaveStatus.ShouldContain("local folder", Case.Insensitive);
        await local.DidNotReceive().CreateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
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
        local.CreateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(@"C:\repo\drafts\pulled-post.md");
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));
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
        await local.Received(1).CreateAsync(
            @"C:\repo", "drafts/pulled-post.md", "pulled-from-editor", Arg.Any<CancellationToken>());
    }

    // ---- Publish / Merge (in-app GitHub: open a PR, then explicitly merge it) ----

    private static EditorViewModel GitHubVm(out InMemoryGitHubGateway gw)
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>())
            .Returns(call => new ParsedDocument(FrontMatter.Default.WithTitle("Hello World"), call.Arg<string>()));
        gw = new InMemoryGitHubGateway();
        gw.SeedBranchAsync("o", "r", "develop", "basesha").GetAwaiter().GetResult();
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default with { Owner = "o", Repo = "r" });
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gw, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gw));
        vm.CancelTitlePromptCommand.Execute(null);   // dismiss the new-post title overlay
        return vm;
    }

    [Fact]
    public async Task Publish_on_a_github_configured_post_opens_a_pr_and_tracks_it()
    {
        var vm = GitHubVm(out _);
        vm.RawMarkdown = "---\ntitle: \"Hello World\"\ndraft: true\n---\n\n# Hi\n";

        await vm.PublishCommand.ExecuteAsync(null);

        vm.PendingPullRequest.ShouldNotBeNull();
        vm.SaveStatus.ShouldContain("PR #");
        vm.MergeCommand.CanExecute(null).ShouldBeTrue("a merge target should exist after publishing");
    }

    [Fact]
    public async Task Publish_without_a_github_repo_hints_and_tracks_no_pr()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(call => new ParsedDocument(FrontMatter.Default.WithTitle("X"), call.Arg<string>()));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stub"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);   // no Owner/Repo
        var local = Substitute.For<ILocalPostStore>();
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));
        vm.CancelTitlePromptCommand.Execute(null);
        vm.RawMarkdown = "body";

        await vm.PublishCommand.ExecuteAsync(null);

        vm.PendingPullRequest.ShouldBeNull();
        vm.SaveStatus.ShouldContain("GitHub repo", Case.Insensitive);
    }

    [Fact]
    public async Task Merge_after_publish_merges_the_tracked_pr_and_clears_it()
    {
        var vm = GitHubVm(out _);
        vm.RawMarkdown = "---\ntitle: \"Hello World\"\ndraft: true\n---\n\n# Hi\n";
        await vm.PublishCommand.ExecuteAsync(null);
        vm.PendingPullRequest.ShouldNotBeNull();

        await vm.MergeCommand.ExecuteAsync(null);

        vm.SaveStatus.ShouldContain("Merged", Case.Insensitive);
        vm.PendingPullRequest.ShouldBeNull();
        vm.MergeCommand.CanExecute(null).ShouldBeFalse("the merge target is consumed once merged");
    }

    [Fact]
    public void Merge_is_disabled_when_no_pr_is_pending()
    {
        var vm = GitHubVm(out _);
        vm.MergeCommand.CanExecute(null).ShouldBeFalse();
    }

    // ---- Malformed front matter while typing must not crash the editor ----
    //
    // OnRawMarkdownChanged runs on every keystroke. While the user is mid-edit the YAML front
    // matter is routinely, transiently invalid (an unterminated quote, a half-typed list) and the
    // parser throws out of Parse. That exception used to propagate out of the RawMarkdown setter:
    // on desktop it escaped the async-void WebView "change" handler (crash); on Android it re-threw
    // out of the ~750ms dirty-poll every tick (PXBRIDGE log spam + frozen WordCount/FrontMatter).
    // The setter must swallow the failure, keep the last-good FrontMatter, and keep the count live.

    private static EditorViewModel NewVm(IFrontMatterParser parser)
    {
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        return new EditorViewModel(parser, gateway, settings, local, TimeProvider.System, StubGit(), new GitHubPublishService(gateway));
    }

    [Fact]
    public void Typing_malformed_frontmatter_does_not_throw_out_of_the_RawMarkdown_setter()
    {
        const string malformed =
            "---\n" +
            "title: \"half-typed quote\n" +   // opens a double-quoted scalar that never closes
            "---\n\n" +
            "body text\n";

        // Guard: the real parser genuinely throws on this input, so the assertion below isn't vacuous.
        Should.Throw<Exception>(() => new YamlFrontMatterParser().Parse(malformed));

        var vm = NewVm(new YamlFrontMatterParser());
        vm.CancelTitlePromptCommand.Execute(null);
        vm.IsDirty = false;

        Should.NotThrow(() => vm.RawMarkdown = malformed);

        vm.IsDirty.ShouldBeTrue("a keystroke still marks the buffer dirty even when the YAML can't parse");
        vm.RawMarkdown.ShouldBe(malformed);
    }

    [Fact]
    public void Malformed_frontmatter_keeps_the_word_count_live_over_the_raw_buffer()
    {
        // 7 whitespace-separated tokens across the whole buffer (---, oops:, "unterminated, ---,
        // alpha, beta, gamma). With the front matter unparseable we can't split it from the body,
        // so the whole raw buffer is counted -- the point is the count stays live, not frozen at 0.
        const string malformed =
            "---\n" +
            "oops: \"unterminated\n" +
            "---\n" +
            "alpha beta gamma\n";

        var vm = NewVm(new YamlFrontMatterParser());
        vm.CancelTitlePromptCommand.Execute(null);

        vm.RawMarkdown = malformed;

        vm.WordCount.ShouldBe(7);
        vm.ReadingTimeMinutes.ShouldBe(1);
    }

    [Fact]
    public void Valid_frontmatter_after_a_malformed_edit_recovers_the_metadata()
    {
        var vm = NewVm(new YamlFrontMatterParser());
        vm.CancelTitlePromptCommand.Execute(null);

        // 1) a clean document parses normally
        vm.RawMarkdown = "---\ntitle: Alpha\n---\n\nfirst body\n";
        vm.FrontMatter.Title.ShouldBe("Alpha");

        // 2) the YAML goes transiently invalid: no throw, and the last-good title is retained
        vm.RawMarkdown = "---\ntitle: \"Alpha and then some\n---\n\nstill editing\n";
        vm.FrontMatter.Title.ShouldBe("Alpha", "a transient parse failure keeps the last-good front matter");

        // 3) the YAML is valid again: metadata recovers from the new front matter
        vm.RawMarkdown = "---\ntitle: Bravo\n---\n\nrecovered body here\n";
        vm.FrontMatter.Title.ShouldBe("Bravo");
        vm.WordCount.ShouldBe(3, "the body word count tracks the recovered document");
    }

    [Fact]
    public void Any_parser_exception_is_swallowed_and_the_last_good_frontmatter_is_kept()
    {
        // The VM defends against *any* IFrontMatterParser throwing, not just YamlDotNet -- the
        // interface doesn't promise Parse won't throw, so the setter must never let it escape.
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(new ParsedDocument(FrontMatter.Default.WithTitle("Good"), "good body"));
        var vm = NewVm(parser);
        vm.CancelTitlePromptCommand.Execute(null);

        vm.RawMarkdown = "anything";
        vm.FrontMatter.Title.ShouldBe("Good");

        parser.Parse("now totally broken text").Throws(new InvalidOperationException("kaboom"));

        Should.NotThrow(() => vm.RawMarkdown = "now totally broken text");

        vm.FrontMatter.Title.ShouldBe("Good", "the last-good front matter survives a parse failure");
        vm.IsDirty.ShouldBeTrue();
        vm.WordCount.ShouldBe(4, "word count falls back to the raw buffer (now, totally, broken, text)");
    }
}
