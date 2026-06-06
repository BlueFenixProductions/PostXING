namespace PostXING.ViewModels.Theming;

/// <summary>The 14 CSS custom-property values the editor's <c>index.html</c> consumes
/// (<c>--canvas</c> … <c>--brace</c>). Values are raw CSS color strings — mostly hex, but
/// <see cref="Selection"/> is an <c>rgba()</c> string — matching the <c>:root</c> declarations 1:1.</summary>
public sealed record EditorPalette
{
    public required string Canvas { get; init; }
    public required string Text { get; init; }
    public required string Comment { get; init; }
    public required string StringLiteral { get; init; }
    public required string Number { get; init; }
    public required string Keyword { get; init; }
    public required string Function { get; init; }
    public required string Variable { get; init; }
    public required string Heading { get; init; }
    public required string Punctuation { get; init; }
    public required string Caret { get; init; }
    public required string Selection { get; init; }
    public required string Bracket { get; init; }
    public required string Brace { get; init; }

    /// <summary>The 14 vars as ordered (css-var-name, value) pairs — deterministic, for the JS
    /// bridge payload and golden tests.</summary>
    public IReadOnlyList<KeyValuePair<string, string>> ToCssVars() =>
    [
        new("--canvas", Canvas), new("--text", Text), new("--comment", Comment),
        new("--string", StringLiteral), new("--number", Number), new("--keyword", Keyword),
        new("--function", Function), new("--variable", Variable), new("--heading", Heading),
        new("--punctuation", Punctuation), new("--caret", Caret), new("--selection", Selection),
        new("--bracket", Bracket), new("--brace", Brace),
    ];
}
