using System.Globalization;
using PostXING.GitHub;

namespace PostXING.App.Converters;

/// <summary>
/// Shared resolution of a <see cref="RepoSyncState"/> to the status-bar sync chip color:
/// Synced = green (Success), NeedsPull = orange (Warning), LocalChanges = blue (PhoenixBlue),
/// Unknown = neutral (Paper500). Resolves the brand tokens from app resources, with hex fallbacks.
/// </summary>
internal static class SyncChipColor
{
    public static Color Resolve(object? value)
    {
        var (key, fallback) = (value as RepoSyncState?) switch
        {
            RepoSyncState.Synced => ("Success", "#10B981"),
            RepoSyncState.NeedsPull => ("Warning", "#F59E0B"),
            RepoSyncState.LocalChanges => ("PhoenixBlue", "#1E5BFF"),
            _ => ("Paper500", "#9095A4"),
        };

        if (Application.Current?.Resources.TryGetValue(key, out var resource) == true && resource is Color color)
            return color;
        return Color.FromArgb(fallback);
    }
}

/// <summary>RepoSyncState -> chip text color.</summary>
public sealed class RepoSyncStateToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => SyncChipColor.Resolve(value);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>RepoSyncState -> chip icon fill, so the GitHub mark matches the chip text color.</summary>
public sealed class RepoSyncStateToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => new SolidColorBrush(SyncChipColor.Resolve(value));

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
