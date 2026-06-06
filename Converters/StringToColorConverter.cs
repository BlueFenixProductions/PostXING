using System.Globalization;

namespace PostXING.App.Converters;

/// <summary>A hex color string (a theme token / palette value) -> a MAUI <see cref="Color"/>, for
/// the theme-picker preview swatches that bind directly to a <c>Theme</c>'s string hexes.</summary>
public sealed class StringToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is string s && !string.IsNullOrWhiteSpace(s) ? Color.FromArgb(s) : Colors.Transparent;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
