using System.Text.Json;
using System.Text.Json.Serialization;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

/// <summary>
/// The first-run seed parser. A gitignored <c>defaults.local.json</c> (same JSON shape as the
/// on-disk settings file) is baked into the build to pre-fill a developer's personal defaults
/// without committing them to the public repo; a missing/blank/corrupt seed must fall back to
/// <see cref="AppSettings.Default"/> so a public clone with no seed gets the neutral defaults.
/// </summary>
public sealed class SettingsSeedTests
{
    private static readonly JsonSerializerOptions StringEnumOptions =
        new() { Converters = { new JsonStringEnumConverter() } };

    [Fact]
    public void Valid_seed_populates_all_fields()
    {
        const string seed =
            """
            {
              "Owner": "ChrisPelatari",
              "Repo": "vitepress.chris.pelatari.com",
              "DevelopBranch": "main",
              "AuthorName": "Chris Pelatari",
              "LocalFolder": null,
              "ContentRoot": "blog",
              "Theme": "Dark"
            }
            """;

        var s = SettingsSeed.ParseOrDefault(seed);

        s.Owner.ShouldBe("ChrisPelatari");
        s.Repo.ShouldBe("vitepress.chris.pelatari.com");
        s.DevelopBranch.ShouldBe("main");
        s.AuthorName.ShouldBe("Chris Pelatari");
        s.ContentRoot.ShouldBe("blog");
        s.PostsPrefix.ShouldBe("blog/posts/");   // the seed's whole point: posts live under blog/
        s.Theme.ShouldBe(ThemeChoice.Dark);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Missing_or_blank_seed_falls_back_to_default(string? json)
    {
        SettingsSeed.ParseOrDefault(json).ShouldBe(AppSettings.Default);
    }

    [Fact]
    public void Corrupt_seed_falls_back_to_default_without_throwing()
    {
        SettingsSeed.ParseOrDefault("{ not valid json ").ShouldBe(AppSettings.Default);
    }

    [Fact]
    public void Round_trips_a_serialized_AppSettings()
    {
        var original = AppSettings.Default with
        {
            Owner = "octocat",
            Repo = "blog",
            DevelopBranch = "main",
            AuthorName = "Octo Cat",
            ContentRoot = "blog",
            Theme = ThemeChoice.Light,
        };
        var json = JsonSerializer.Serialize(original, StringEnumOptions);

        SettingsSeed.ParseOrDefault(json).ShouldBe(original);
    }
}
