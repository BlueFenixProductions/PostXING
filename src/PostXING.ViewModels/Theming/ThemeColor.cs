using PostXING.Core.Theming;

namespace PostXING.ViewModels.Theming;

/// <summary>Pure CSS-color validation for theme data. Accepts hex (delegated to Core's
/// <see cref="Contrast.IsValidHex"/>) and <c>rgb()/rgba()</c> functional notation — the editor's
/// <c>--selection</c> var is an rgba string.</summary>
public static class ThemeColor
{
    public static bool IsValidCss(string? s) =>
        Contrast.IsValidHex(s) ||
        (s is not null &&
            (s.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase) ||
             s.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase)) &&
            s.EndsWith(')'));
}
