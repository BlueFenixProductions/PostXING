namespace PostXING.ViewModels.Theming;

/// <summary>The colors the GitHub-markdown preview render needs. <see cref="Canvas"/>/<see cref="Fg"/>
/// replace the formerly hardcoded <c>#0d1117</c>/<c>#f0f6fc</c>; the rest recolor links, heading
/// rules, inline/fenced code backgrounds, and table/hr borders per theme (instead of one CSS file
/// per theme).</summary>
public sealed record PreviewPalette
{
    public required string Canvas { get; init; }
    public required string Fg { get; init; }
    public required string Accent { get; init; }
    public required string Link { get; init; }
    public required string CodeBg { get; init; }
    public required string Border { get; init; }
}
