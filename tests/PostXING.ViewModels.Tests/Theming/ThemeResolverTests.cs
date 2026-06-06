using PostXING.ViewModels;
using PostXING.ViewModels.Theming;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests.Theming;

public sealed class ThemeResolverTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Sync_off_returns_the_explicit_theme_id(bool osIsDark) =>
        ThemeResolver.Resolve(AppSettings.Default with { SyncWithOs = false, ThemeId = "dracula" }, osIsDark)
            .ShouldBe("dracula");

    [Fact]
    public void Sync_on_and_os_dark_returns_the_dark_theme_id() =>
        ThemeResolver.Resolve(
            AppSettings.Default with { SyncWithOs = true, DarkThemeId = "tokyo-night", LightThemeId = "github-light" },
            osIsDark: true).ShouldBe("tokyo-night");

    [Fact]
    public void Sync_on_and_os_light_returns_the_light_theme_id() =>
        ThemeResolver.Resolve(
            AppSettings.Default with { SyncWithOs = true, DarkThemeId = "tokyo-night", LightThemeId = "github-light" },
            osIsDark: false).ShouldBe("github-light");

    [Fact]
    public void Unknown_theme_id_resolves_to_the_default() =>
        ThemeResolver.Resolve(AppSettings.Default with { SyncWithOs = false, ThemeId = "no-such-theme" }, true)
            .ShouldBe(ThemeCatalog.DefaultId);
}
