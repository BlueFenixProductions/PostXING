using NSubstitute;
using PostXING.GitHub;
using PostXING.ViewModels;
using Shouldly;
using Xunit;

namespace PostXING.ViewModels.Tests;

/// <summary>
/// The PAT path: a pasted token is persisted only if it actually authenticates. The gateway is
/// faked (its real validation is covered in <c>HttpGitHubGatewayTests</c>); these lock the
/// view-model orchestration — store, validate, roll back on failure, never keep the secret in the
/// bound field.
/// </summary>
public sealed class SettingsViewModelTests
{
    private static (SettingsViewModel vm, IGitHubGateway gateway, InMemoryGitHubTokenStore tokens) CreateVm(string? existingToken = null)
    {
        var store = Substitute.For<ISettingsStore>();
        store.Current.Returns(AppSettings.Default);
        var gateway = Substitute.For<IGitHubGateway>();
        // Ctor fires a CheckAuth; give it a real status so the fire-and-forget doesn't NRE.
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "no token"));
        var picker = Substitute.For<IFolderPicker>();
        var tokens = new InMemoryGitHubTokenStore(existingToken);
        return (new SettingsViewModel(store, gateway, picker, tokens), gateway, tokens);
    }

    [Fact]
    public async Task PasteToken_with_valid_token_stores_it_and_authenticates()
    {
        var (vm, gateway, tokens) = CreateVm();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(true, "chris", "Authenticated as chris."));
        vm.TokenInput = "ghp_valid";

        await vm.PasteTokenCommand.ExecuteAsync(null);

        (await tokens.GetTokenAsync()).ShouldBe("ghp_valid");
        vm.IsAuthenticated.ShouldBeTrue();
        vm.TokenInput.ShouldBeEmpty();   // secret must not linger in the bound field
    }

    [Fact]
    public async Task PasteToken_that_fails_keeps_the_token_and_field_for_retry()
    {
        var (vm, gateway, tokens) = CreateVm();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "Bad credentials"));
        vm.TokenInput = "ghp_maybe_transient";

        await vm.PasteTokenCommand.ExecuteAsync(null);

        // A failed check must NOT wipe the token - a transient failure shouldn't lose a valid PAT;
        // only Disconnect clears it. The field stays populated so the user can reveal/fix it.
        (await tokens.GetTokenAsync()).ShouldBe("ghp_maybe_transient");
        vm.IsAuthenticated.ShouldBeFalse();
        vm.TokenInput.ShouldBe("ghp_maybe_transient");
    }

    [Fact]
    public void ToggleTokenVisibility_flips_masking()
    {
        var (vm, _, _) = CreateVm();
        vm.MaskToken.ShouldBeTrue();    // masked by default

        vm.ToggleTokenVisibilityCommand.Execute(null);

        vm.ShowToken.ShouldBeTrue();
        vm.MaskToken.ShouldBeFalse();
    }

    [Fact]
    public async Task PasteToken_with_blank_input_does_not_touch_the_store()
    {
        var (vm, _, tokens) = CreateVm();
        vm.TokenInput = "   ";

        await vm.PasteTokenCommand.ExecuteAsync(null);

        (await tokens.GetTokenAsync()).ShouldBeNull();
    }

    [Fact]
    public async Task Disconnect_clears_the_stored_token()
    {
        var (vm, _, tokens) = CreateVm(existingToken: "ghp_existing");

        await vm.DisconnectCommand.ExecuteAsync(null);

        (await tokens.GetTokenAsync()).ShouldBeNull();
        vm.IsAuthenticated.ShouldBeFalse();
    }

    private static (SettingsViewModel vm, ISettingsStore store) CreateVmWithStore(
        AppSettings? current = null, Func<bool>? osIsDark = null)
    {
        var store = Substitute.For<ISettingsStore>();
        store.Current.Returns(current ?? AppSettings.Default);
        var gateway = Substitute.For<IGitHubGateway>();
        gateway.CheckAuthAsync(Arg.Any<CancellationToken>()).Returns(new GhAuthStatus(false, null, "no token"));
        var vm = new SettingsViewModel(store, gateway, Substitute.For<IFolderPicker>(), new InMemoryGitHubTokenStore(), osIsDark);
        return (vm, store);
    }

    [Fact]
    public void Theme_id_loads_from_the_store()
    {
        var (vm, _) = CreateVmWithStore(AppSettings.Default with { ThemeId = "dracula", ThemeMigrated = true });
        vm.SelectedThemeId.ShouldBe("dracula");
    }

    [Fact]
    public void First_run_selection_defaults_to_phoenix()
    {
        var (vm, _) = CreateVmWithStore();
        vm.SelectedThemeId.ShouldBe("phoenix");
    }

    [Fact]
    public void Selecting_a_theme_applies_the_resolved_id_and_persists()
    {
        // Instant + sticky: raises the apply event for the view AND writes through to the store.
        var (vm, store) = CreateVmWithStore();
        string? applied = null;
        vm.ThemeApplyRequested += (_, id) => applied = id;

        vm.SelectThemeCommand.Execute("solarized-dark");

        applied.ShouldBe("solarized-dark");
        store.Received(1).SaveAsync(
            Arg.Is<AppSettings>(s => s.ThemeId == "solarized-dark" && s.ThemeMigrated), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Sync_on_applies_the_pair_member_for_the_os_brightness()
    {
        var (vm, store) = CreateVmWithStore(osIsDark: () => true); // OS reports dark
        vm.DarkThemeId = "tokyo-night";
        string? applied = null;
        vm.ThemeApplyRequested += (_, id) => applied = id;

        vm.SyncWithOs = true;

        applied.ShouldBe("tokyo-night"); // sync on + OS dark -> the dark pair member
        store.Received().SaveAsync(Arg.Is<AppSettings>(s => s.SyncWithOs), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Choosing_pair_themes_persists_them()
    {
        var (vm, store) = CreateVmWithStore();
        vm.LightThemeId = "light-owl";
        vm.DarkThemeId = "nord";

        store.Received().SaveAsync(Arg.Is<AppSettings>(s => s.LightThemeId == "light-owl"), Arg.Any<CancellationToken>());
        store.Received().SaveAsync(Arg.Is<AppSettings>(s => s.DarkThemeId == "nord"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Save_carries_the_theme_fields()
    {
        var (vm, store) = CreateVmWithStore();
        vm.SelectThemeCommand.Execute("gruvbox");

        await vm.SaveCommand.ExecuteAsync(null);

        await store.Received().SaveAsync(
            Arg.Is<AppSettings>(s => s.ThemeId == "gruvbox" && s.ThemeMigrated), Arg.Any<CancellationToken>());
    }
}
