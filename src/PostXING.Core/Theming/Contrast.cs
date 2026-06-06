using System.Globalization;

namespace PostXING.Core.Theming;

/// <summary>
/// WCAG 2.x relative-luminance and contrast-ratio math over hex colors. Pure, dependency-free
/// (no MAUI types) so it lives in Core and is unit-testable off the MAUI TFM. Powers the theme
/// catalog's accessibility gate: every curated theme's text/accent contrast is asserted in tests.
/// </summary>
public static class Contrast
{
    /// <summary>
    /// WCAG contrast ratio between two colors: (L_lighter + 0.05) / (L_darker + 0.05).
    /// Ranges from 1.0 (identical colors) to 21.0 (pure black vs pure white). Symmetric in its args.
    /// </summary>
    public static double Ratio(string a, string b)
    {
        var la = RelativeLuminance(a);
        var lb = RelativeLuminance(b);
        var (hi, lo) = la >= lb ? (la, lb) : (lb, la);
        return (hi + 0.05) / (lo + 0.05);
    }

    /// <summary>WCAG relative luminance (0..1) of a hex color, sRGB gamma-linearized.</summary>
    public static double RelativeLuminance(string hex)
    {
        var (r, g, b) = ParseRgb(hex);
        return (0.2126 * Linear(r)) + (0.7152 * Linear(g)) + (0.0722 * Linear(b));
    }

    /// <summary>
    /// True if the string is a hex color — #rgb, #rgba, #rrggbb, or #rrggbbaa, with the leading
    /// '#' optional. Alpha digits are accepted (themes' editor --selection can be 8-digit) but
    /// ignored by the luminance math.
    /// </summary>
    public static bool IsValidHex(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        var h = s.StartsWith('#') ? s[1..] : s;
        if (h.Length is not (3 or 4 or 6 or 8)) return false;
        foreach (var c in h)
            if (!Uri.IsHexDigit(c)) return false;
        return true;
    }

    // sRGB channel (0..1) -> linear-light value.
    private static double Linear(double channel) =>
        channel <= 0.03928 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);

    private static (double R, double G, double B) ParseRgb(string hex)
    {
        if (!IsValidHex(hex))
            throw new FormatException($"'{hex}' is not a valid hex color.");
        var h = hex.StartsWith('#') ? hex[1..] : hex;
        if (h.Length is 3 or 4) // expand shorthand: #rgb -> #rrggbb, #rgba -> #rrggbbaa
            h = string.Concat(h.Select(c => new string(c, 2)));
        var r = byte.Parse(h.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var g = byte.Parse(h.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var b = byte.Parse(h.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        return (r / 255.0, g / 255.0, b / 255.0);
    }
}
