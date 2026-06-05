using System.Text.Json;
using PostXING.ViewModels;

namespace PostXING.App.Services;

public sealed class FileSystemSettingsStore : ISettingsStore
{
    // Shared for load AND save so the enum theme writes/reads as a readable name ("Dark") rather
    // than an int, and a future enum reorder can't corrupt existing files.
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
    };

    private readonly string _path;
    private AppSettings _current = AppSettings.Default;

    public AppSettings Current => _current;
    public event EventHandler? Changed;

    public FileSystemSettingsStore()
    {
#if ANDROID
        // Android scoped storage: settings live in the app-private data dir.
        var dir = Microsoft.Maui.Storage.FileSystem.AppDataDirectory;
#else
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PostXING");
#endif
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "settings.json");
    }

    public Task LoadAsync(CancellationToken ct = default)
    {
        // Synchronous on purpose. The DI registration resolves this store with a blocking
        // GetAwaiter().GetResult(), and now that the App constructor depends on ISettingsStore that
        // resolve runs during App construction on the main thread. An awaited async read there
        // (DeserializeAsync) deadlocks the main thread whenever a settings.json already exists,
        // leaving the app stuck on the blue splash. The file is tiny; a sync read is correct and
        // deadlock-free.
        if (!File.Exists(_path))
        {
            // No settings yet (fresh install / post-wipe): seed from the embedded per-developer
            // defaults if one was baked in, else stay on AppSettings.Default. Nothing is written to
            // disk here - the seeded values become Current and persist on the first Save.
            _current = SettingsSeed.ParseOrDefault(ReadEmbeddedSeed());
            return Task.CompletedTask;
        }
        try
        {
            var json = File.ReadAllText(_path);
            var loaded = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
            if (loaded is not null)
            {
                _current = loaded;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (JsonException)
        {
            // Corrupt settings file; keep defaults.
        }
        return Task.CompletedTask;
    }

    // Reads the per-developer first-run seed baked in from a gitignored defaults.local.json (see
    // PostXING.App.csproj). Returns null on a public clone / CI build where no seed was embedded, so
    // the caller falls back to AppSettings.Default. Synchronous to honor LoadAsync's deadlock-safe
    // contract (it runs via a blocking resolve on the main thread during App construction).
    private static string? ReadEmbeddedSeed()
    {
        using var stream = typeof(FileSystemSettingsStore).Assembly
            .GetManifestResourceStream("PostXING.App.defaults.local.json");
        if (stream is null) return null;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken ct = default)
    {
        _current = settings;
        await using var fs = File.Create(_path);
        await JsonSerializer.SerializeAsync(fs, settings, JsonOptions, ct);
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
