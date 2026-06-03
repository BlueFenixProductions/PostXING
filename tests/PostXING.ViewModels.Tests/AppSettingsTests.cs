using System.Text.Json;
using System.Text.Json.Serialization;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

public sealed class AppSettingsTests
{
    private static readonly JsonSerializerOptions StringEnumOptions =
        new() { Converters = { new JsonStringEnumConverter() } };

    [Fact]
    public void Default_content_root_lists_top_level_post_folders()
    {
        AppSettings.Default.PostsPrefix.ShouldBe("posts/");
        AppSettings.Default.DraftsPrefix.ShouldBe("drafts/");
    }

    [Fact]
    public void Default_theme_is_dark()
    {
        // DESIGN.md: dark canvas is the brand default; light is the writer's-daylight opt-in.
        AppSettings.Default.Theme.ShouldBe(ThemeChoice.Dark);
    }

    [Fact]
    public void Theme_round_trips_through_json()
    {
        var json = JsonSerializer.Serialize(AppSettings.Default with { Theme = ThemeChoice.Light });
        JsonSerializer.Deserialize<AppSettings>(json)!.Theme.ShouldBe(ThemeChoice.Light);
    }

    [Fact]
    public void Settings_json_without_a_theme_field_defaults_to_dark()
    {
        // Back-compat: a settings.json written before the theme field existed must load as Dark.
        const string legacy =
            """{"Owner":null,"Repo":null,"DevelopBranch":"develop","AuthorName":null,"LocalFolder":null,"ContentRoot":null}""";
        JsonSerializer.Deserialize<AppSettings>(legacy)!.Theme.ShouldBe(ThemeChoice.Dark);
    }

    [Fact]
    public void Theme_persists_as_a_readable_name_with_the_store_converter()
    {
        // Mirrors FileSystemSettingsStore's options: the theme is written as "Light", not an int,
        // so the file stays readable and an enum reorder can't corrupt it.
        JsonSerializer.Serialize(AppSettings.Default with { Theme = ThemeChoice.Light }, StringEnumOptions)
            .ShouldContain("\"Light\"");
    }

    [Theory]
    [InlineData("blog", "blog/posts/", "blog/drafts/")]
    [InlineData("blog/", "blog/posts/", "blog/drafts/")]
    [InlineData("/blog/", "blog/posts/", "blog/drafts/")]
    [InlineData("  blog  ", "blog/posts/", "blog/drafts/")]
    [InlineData("content/site", "content/site/posts/", "content/site/drafts/")]
    public void Content_root_prefixes_post_and_draft_folders(string root, string posts, string drafts)
    {
        var s = AppSettings.Default with { ContentRoot = root };
        s.PostsPrefix.ShouldBe(posts);
        s.DraftsPrefix.ShouldBe(drafts);
    }
}
