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
    /// <summary>
    /// Locks the EditorViewModel ctor signature. PreviewHtml + IMarkdownRenderer were deleted
    /// in the "no dead Markdig render on every keystroke" optimization pass; this test exists so
    /// re-adding either dependency requires a deliberate code change accompanied by an updated test.
    /// </summary>
    [Fact]
    public void Ctor_takes_only_the_five_real_dependencies()
    {
        var parser = Substitute.For<IFrontMatterParser>();
        parser.Parse(Arg.Any<string>()).Returns(new ParsedDocument(FrontMatter.Default, string.Empty));
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "stubbed"));
        var settings = Substitute.For<ISettingsStore>();
        settings.Current.Returns(AppSettings.Default);
        var local = Substitute.For<ILocalPostStore>();
        var clock = TimeProvider.System;

        var vm = new EditorViewModel(parser, gateway, settings, local, clock);

        vm.ShouldNotBeNull();
        vm.ShowTitlePrompt.ShouldBeTrue("New EditorViewModel begins with the title-prompt overlay open.");
        vm.IsDirty.ShouldBeFalse();
        vm.CurrentPath.ShouldBe("(new)");
        vm.RawMarkdown.ShouldBe(string.Empty);
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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System);
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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System);

        vm.LoadPost(PostHandle.FromLocalPath(@"C:\posts\drafts\hello.md"), "# Hello\n");

        vm.IsDirty.ShouldBeFalse("Opening an existing post must not mark the buffer dirty.");
        vm.CurrentPath.ShouldContain("hello.md");
        vm.ShowTitlePrompt.ShouldBeFalse();
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
        var vm = new EditorViewModel(parser, gateway, settings, local, TimeProvider.System);
        vm.CancelTitlePromptCommand.Execute(null);
        vm.RawMarkdown = "some content";

        await vm.SaveCommand.ExecuteAsync(null);

        vm.SaveStatus.ShouldContain("local folder", Case.Insensitive);
        await local.DidNotReceive().WriteAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
