using System.Linq;
using PostXING.Markdown;
using Shouldly;
using Xunit;

namespace PostXING.Markdown.Tests;

public sealed class FrontMatterEditorTests
{
    // The shape EditorViewModel's title prompt seeds: draft: true inside a --- fence.
    private const string Seeded =
        "---\ntitle: \"Hello\"\ndate: 2026-06-01\ndraft: true\ntags: []\n---\n\n# Hello\n";

    [Fact]
    public void WithDraft_flips_true_to_false()
    {
        var result = FrontMatterEditor.WithDraft(Seeded, draft: false);
        result.ShouldContain("draft: false");
        result.ShouldNotContain("draft: true");
    }

    [Fact]
    public void WithDraft_preserves_other_front_matter_and_body()
    {
        var result = FrontMatterEditor.WithDraft(Seeded, draft: false);
        result.ShouldContain("title: \"Hello\"");
        result.ShouldContain("tags: []");
        result.ShouldContain("# Hello");
    }

    [Fact]
    public void WithDraft_tolerates_missing_space_after_colon()
    {
        var raw = "---\ndraft:true\n---\n";
        FrontMatterEditor.WithDraft(raw, draft: false).ShouldContain("draft: false");
    }

    [Fact]
    public void WithDraft_already_false_stays_a_single_false_line()
    {
        var raw = "---\ndraft: false\n---\n";
        var result = FrontMatterEditor.WithDraft(raw, draft: false);
        result.Split('\n').Count(l => l.TrimStart().StartsWith("draft:", StringComparison.Ordinal)).ShouldBe(1);
        result.ShouldContain("draft: false");
    }

    [Fact]
    public void WithDraft_inserts_a_draft_key_when_the_fence_has_none()
    {
        var raw = "---\ntitle: \"X\"\n---\n\nbody\n";
        var result = FrontMatterEditor.WithDraft(raw, draft: false);
        result.ShouldContain("draft: false");
        result.ShouldContain("title: \"X\"");
    }

    [Fact]
    public void WithDraft_returns_unchanged_when_there_is_no_front_matter()
    {
        var raw = "# Just a heading\n\nNo front matter here.\n";
        FrontMatterEditor.WithDraft(raw, draft: false).ShouldBe(raw);
    }
}
