using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostXING.ViewModels;

/// <summary>
/// Parses a first-run seed (the same JSON shape as the on-disk settings file) into
/// <see cref="AppSettings"/>. The seed lets a developer pre-fill their personal defaults on a fresh
/// install without committing them to the public repo: <c>defaults.local.json</c> is gitignored and
/// only baked into the build when present (see PostXING.App.csproj). A missing, blank, or corrupt
/// seed falls back to <see cref="AppSettings.Default"/>, so a public clone (no seed) simply gets the
/// neutral defaults.
/// </summary>
public static class SettingsSeed
{
    // Mirrors FileSystemSettingsStore's options so the theme reads as a readable name ("Dark")
    // rather than an int, matching how settings.json itself is written.
    private static readonly JsonSerializerOptions Options =
        new() { Converters = { new JsonStringEnumConverter() } };

    public static AppSettings ParseOrDefault(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return AppSettings.Default;
        try
        {
            return JsonSerializer.Deserialize<AppSettings>(json, Options) ?? AppSettings.Default;
        }
        catch (JsonException)
        {
            return AppSettings.Default;
        }
    }
}
