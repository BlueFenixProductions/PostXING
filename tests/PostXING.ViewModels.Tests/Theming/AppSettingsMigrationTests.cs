using System.Text.Json;
using System.Text.Json.Serialization;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests.Theming;

public sealed class AppSettingsMigrationTests
{
    private static readonly JsonSerializerOptions StringEnum =
        new() { Converters = { new JsonStringEnumConverter() } };

    [Fact]
    public void Default_gallery_fields()
    {
        AppSettings.Default.ThemeId.ShouldBe("phoenix");
        AppSettings.Default.SyncWithOs.ShouldBeFalse();
        AppSettings.Default.LightThemeId.ShouldBe("phoenix-light");
        AppSettings.Default.DarkThemeId.ShouldBe("phoenix");
    }

    [Fact]
    public void Migrate_dark_maps_to_dark_theme_id()
    {
        var m = (AppSettings.Default with { Theme = ThemeChoice.Dark, ThemeMigrated = false }).Migrate();
        m.ThemeId.ShouldBe(AppSettings.Default.DarkThemeId);
        m.SyncWithOs.ShouldBeFalse();
        m.ThemeMigrated.ShouldBeTrue();
    }

    [Fact]
    public void Migrate_light_maps_to_light_theme_id() =>
        (AppSettings.Default with { Theme = ThemeChoice.Light, ThemeMigrated = false })
            .Migrate().ThemeId.ShouldBe(AppSettings.Default.LightThemeId);

    [Fact]
    public void Migrate_system_turns_sync_on()
    {
        var m = (AppSettings.Default with { Theme = ThemeChoice.System, ThemeMigrated = false }).Migrate();
        m.SyncWithOs.ShouldBeTrue();
        m.ThemeMigrated.ShouldBeTrue();
    }

    [Fact]
    public void Migrate_is_idempotent()
    {
        var once = (AppSettings.Default with { Theme = ThemeChoice.Light, ThemeMigrated = false }).Migrate();
        once.Migrate().ShouldBe(once); // second pass is a no-op
    }

    [Fact]
    public void Migrate_does_not_clobber_an_explicit_choice()
    {
        var chosen = AppSettings.Default with { ThemeId = "dracula", Theme = ThemeChoice.Light, ThemeMigrated = true };
        chosen.Migrate().ThemeId.ShouldBe("dracula");
    }

    [Fact]
    public void Legacy_json_migrates_light_to_light_theme()
    {
        // A pre-gallery settings.json: carries Theme="Light", lacks every gallery field.
        const string legacy =
            """{"Owner":null,"Repo":null,"DevelopBranch":"develop","AuthorName":null,"LocalFolder":null,"ContentRoot":null,"Theme":"Light"}""";
        var loaded = JsonSerializer.Deserialize<AppSettings>(legacy, StringEnum)!;
        loaded.ThemeId.ShouldBe("phoenix");   // raw deserialize falls to record default
        loaded.ThemeMigrated.ShouldBeFalse(); // legacy file lacks the flag
        loaded.Migrate().ThemeId.ShouldBe("phoenix-light");
    }

    [Fact]
    public void New_gallery_fields_round_trip_through_json()
    {
        var s = AppSettings.Default with
        {
            ThemeId = "dracula", SyncWithOs = true, DarkThemeId = "nord", ThemeMigrated = true,
        };
        var back = JsonSerializer.Deserialize<AppSettings>(JsonSerializer.Serialize(s, StringEnum), StringEnum)!;
        back.ThemeId.ShouldBe("dracula");
        back.SyncWithOs.ShouldBeTrue();
        back.DarkThemeId.ShouldBe("nord");
        back.ThemeMigrated.ShouldBeTrue();
    }
}
