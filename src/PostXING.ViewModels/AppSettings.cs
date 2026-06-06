namespace PostXING.ViewModels;

public sealed record AppSettings(
    string? Owner,
    string? Repo,
    string DevelopBranch,
    string? AuthorName,
    string? LocalFolder,
    string? ContentRoot = null,
    // Legacy brightness preference. Kept ONLY as migration input + the first-run seed shape; the
    // app no longer reads it for behavior once the gallery fields below are populated.
    ThemeChoice Theme = ThemeChoice.Dark,
    // --- theme gallery ---
    string ThemeId = "phoenix",           // the explicitly chosen theme (used when SyncWithOs is off)
    bool SyncWithOs = false,               // auto-flip between the light/dark pair on OS brightness
    string LightThemeId = "phoenix-light", // the light member of the sync pair
    string DarkThemeId = "phoenix",        // the dark member of the sync pair
    bool ThemeMigrated = false)            // set once Migrate() has run, so it never re-fires
{
    public static AppSettings Default { get; } = new(
        Owner: null,
        Repo: null,
        DevelopBranch: "develop",
        AuthorName: null,
        LocalFolder: null,
        ContentRoot: null,
        Theme: ThemeChoice.Dark,
        ThemeId: "phoenix",
        SyncWithOs: false,
        LightThemeId: "phoenix-light",
        DarkThemeId: "phoenix",
        ThemeMigrated: false);

    public bool IsGitHubConfigured => !string.IsNullOrWhiteSpace(Owner) && !string.IsNullOrWhiteSpace(Repo);
    // Non-empty is enough: the desktop store handles a missing dir by returning an empty list,
    // and a SAF content:// tree URI (Android) wouldn't satisfy Directory.Exists in any case.
    public bool IsLocalConfigured => !string.IsNullOrWhiteSpace(LocalFolder);

    // The repo subfolder that holds the drafts/ + posts/ trees (e.g. "blog" -> blog/posts/);
    // empty/null = repo root. The drafts/posts/ names and {date}-{slug} convention are unchanged.
    public string PostsPrefix => Prefix("posts/");
    public string DraftsPrefix => Prefix("drafts/");

    private string Prefix(string folder) =>
        string.IsNullOrWhiteSpace(ContentRoot) ? folder : $"{ContentRoot.Trim().Trim('/')}/{folder}";

    /// <summary>One-time normalize on load: derive the gallery fields from the legacy
    /// <see cref="Theme"/> enum when a settings.json predates the theme gallery (System ->
    /// Sync-with-OS on; Light/Dark -> the matching pair member). Idempotent via
    /// <see cref="ThemeMigrated"/>, so it never clobbers an explicit choice made after the upgrade.</summary>
    public AppSettings Migrate()
    {
        if (ThemeMigrated) return this;
        var next = Theme switch
        {
            ThemeChoice.System => this with { SyncWithOs = true },
            ThemeChoice.Light => this with { ThemeId = LightThemeId },
            _ => this with { ThemeId = DarkThemeId }, // Dark (and any future default)
        };
        return next with { ThemeMigrated = true };
    }
}
