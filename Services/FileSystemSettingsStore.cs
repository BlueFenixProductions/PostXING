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

    public async Task LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_path)) return;
        try
        {
            await using var fs = File.OpenRead(_path);
            var loaded = await JsonSerializer.DeserializeAsync<AppSettings>(fs, JsonOptions, ct);
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
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken ct = default)
    {
        _current = settings;
        await using var fs = File.Create(_path);
        await JsonSerializer.SerializeAsync(fs, settings, JsonOptions, ct);
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
