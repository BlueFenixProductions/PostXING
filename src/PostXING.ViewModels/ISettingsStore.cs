namespace PostXING.ViewModels;

public interface ISettingsStore
{
    AppSettings Current { get; }
    Task LoadAsync(CancellationToken ct = default);
    Task SaveAsync(AppSettings settings, CancellationToken ct = default);
    event EventHandler? Changed;
}
