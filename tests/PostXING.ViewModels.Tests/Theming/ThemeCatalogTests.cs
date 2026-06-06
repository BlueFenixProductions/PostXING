using PostXING.Core.Theming;
using PostXING.ViewModels.Theming;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests.Theming;

public sealed class ThemeCatalogTests
{
    public static IEnumerable<object[]> AllThemes => ThemeCatalog.All.Select(t => new object[] { t });

    [Fact]
    public void Catalog_has_at_least_19_themes() =>
        ThemeCatalog.All.Count.ShouldBeGreaterThanOrEqualTo(19);

    [Fact]
    public void Default_is_phoenix()
    {
        ThemeCatalog.Default.Id.ShouldBe("phoenix");
        ThemeCatalog.Default.ShouldBe(ThemeCatalog.Get("phoenix"));
    }

    [Fact]
    public void Phoenix_is_first_in_picker_order() => ThemeCatalog.All[0].Id.ShouldBe("phoenix");

    [Fact]
    public void Ids_are_unique_and_kebab_case()
    {
        var ids = ThemeCatalog.All.Select(t => t.Id).ToList();
        ids.Distinct(StringComparer.OrdinalIgnoreCase).Count().ShouldBe(ids.Count);
        foreach (var id in ids)
            id.ShouldMatch("^[a-z0-9]+(-[a-z0-9]+)*$");
    }

    [Fact]
    public void Unknown_or_null_id_falls_back_to_default()
    {
        ThemeCatalog.Get("does-not-exist").ShouldBe(ThemeCatalog.Default);
        ThemeCatalog.Get(null).ShouldBe(ThemeCatalog.Default);
    }

    [Fact]
    public void Has_at_least_one_light_and_one_dark()
    {
        ThemeCatalog.All.ShouldContain(t => t.Brightness == Brightness.Dark);
        ThemeCatalog.All.ShouldContain(t => t.Brightness == Brightness.Light);
    }

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Every_theme_fills_the_contract(Theme theme) => theme.Validate().ShouldBeEmpty();

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Every_theme_defines_every_native_token(Theme theme) =>
        theme.NativeTokens.Keys.ShouldBe(ThemeCatalog.RequiredNativeTokens, ignoreOrder: true);

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Every_theme_defines_all_14_editor_vars(Theme theme) =>
        theme.Editor.ToCssVars().Select(v => v.Key).ShouldBe(ThemeCatalog.RequiredEditorVars, ignoreOrder: true);

    [Theory]
    [MemberData(nameof(AllThemes))]
    public void Declared_brightness_matches_background_luminance(Theme theme)
    {
        var expected = Contrast.RelativeLuminance(theme.Background) < 0.5 ? Brightness.Dark : Brightness.Light;
        theme.Brightness.ShouldBe(expected);
    }

    [Fact]
    public void Phoenix_seeds_todays_values()
    {
        var p = ThemeCatalog.Get("phoenix");
        p.Background.ShouldBe("#0F0F1F");
        p.PrimaryText.ShouldBe("#E5E7EB");
        p.Editor.Canvas.ShouldBe("#0d1117");
        p.Preview.Canvas.ShouldBe("#0d1117");
    }
}
