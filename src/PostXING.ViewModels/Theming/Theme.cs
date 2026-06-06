using System.Collections.Immutable;

namespace PostXING.ViewModels.Theming;

/// <summary>One curated theme: identity + brightness + the three coordinated surface palettes
/// (native MAUI tokens, the editor CSS vars, the markdown preview). Pure data; <see cref="ThemeCatalog"/>
/// owns the instances. <see cref="NativeTokens"/> keys are the semantic token names listed in
/// <see cref="ThemeCatalog.RequiredNativeTokens"/> (page bg, primary text, accents, outline, …).</summary>
public sealed record Theme
{
    /// <summary>Stable lowercase-kebab id, persisted in settings and used for catalog lookup
    /// (e.g. "tokyo-night"). Never localized; <see cref="Name"/> is the display string.</summary>
    public required string Id { get; init; }

    public required string Name { get; init; }
    public required Brightness Brightness { get; init; }

    /// <summary>Token-name → color-hex map; must contain every key in
    /// <see cref="ThemeCatalog.RequiredNativeTokens"/>.</summary>
    public required ImmutableDictionary<string, string> NativeTokens { get; init; }

    /// <summary>The single non-color native token: the splash/badge glow opacity (0..1).</summary>
    public required double GlowOpacity { get; init; }

    public required EditorPalette Editor { get; init; }
    public required PreviewPalette Preview { get; init; }

    // Named role accessors used by the contrast gate (so tests read roles, not raw keys).
    public string Background => NativeTokens["Surface"];
    public string PrimaryText => NativeTokens["OnSurface"];
    public string PrimaryButton => NativeTokens["Primary"];
    public string OnPrimary => NativeTokens["OnPrimary"];
    public string Kicker => NativeTokens["KickerAccent"];

    /// <summary>Pure self-check used by the contract test: id/name present, every required native
    /// token present and a valid color, every editor var and preview color valid. Returns the list
    /// of problems (empty = valid) so a failing theme names itself in the assertion.</summary>
    public IReadOnlyList<string> Validate()
    {
        var errs = new List<string>();
        if (string.IsNullOrWhiteSpace(Id)) errs.Add("Id is blank");
        if (string.IsNullOrWhiteSpace(Name)) errs.Add($"{Id}: Name is blank");

        foreach (var key in ThemeCatalog.RequiredNativeTokens)
        {
            if (!NativeTokens.TryGetValue(key, out var v))
                errs.Add($"{Id}: missing native token '{key}'");
            else if (!ThemeColor.IsValidCss(v))
                errs.Add($"{Id}: native token '{key}'='{v}' is not a color");
        }

        foreach (var (k, v) in Editor.ToCssVars())
            if (!ThemeColor.IsValidCss(v))
                errs.Add($"{Id}: editor var '{k}'='{v}' is not a color");

        foreach (var (k, v) in new[]
        {
            ("canvas", Preview.Canvas), ("fg", Preview.Fg), ("accent", Preview.Accent),
            ("link", Preview.Link), ("codeBg", Preview.CodeBg), ("border", Preview.Border),
        })
            if (!ThemeColor.IsValidCss(v))
                errs.Add($"{Id}: preview '{k}'='{v}' is not a color");

        return errs;
    }
}
