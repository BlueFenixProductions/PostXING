using System.Text.RegularExpressions;

namespace PostXING.Core.Domain;

public sealed partial record Slug
{
    public string Value { get; }

    private Slug(string value) => Value = value;

    public static Slug From(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        var lowered = title.Trim().ToLowerInvariant();
        var alphanum = NonSlugChars().Replace(lowered, " ");
        var collapsed = Whitespace().Replace(alphanum, "-").Trim('-');
        if (collapsed.Length == 0)
            throw new ArgumentException("Title contains no slug-legal characters.", nameof(title));
        return new Slug(collapsed);
    }

    public static implicit operator string(Slug s) => s.Value;
    public override string ToString() => Value;

    [GeneratedRegex("[^a-z0-9\\s-]")]
    private static partial Regex NonSlugChars();

    [GeneratedRegex("[\\s-]+")]
    private static partial Regex Whitespace();
}
