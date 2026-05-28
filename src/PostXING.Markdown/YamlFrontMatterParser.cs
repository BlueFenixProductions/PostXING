using System.Collections.Immutable;
using PostXING.Core.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PostXING.Markdown;

public sealed class YamlFrontMatterParser : IFrontMatterParser
{
    private const string Delim = "---";

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(LowerCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(LowerCaseNamingConvention.Instance)
        .DisableAliases()
        .Build();

    public ParsedDocument Parse(string document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var span = document.AsSpan().TrimStart();
        if (!span.StartsWith(Delim))
            return new ParsedDocument(FrontMatter.Default.WithTitle(string.Empty), document);

        var firstNewline = document.IndexOf('\n', document.IndexOf(Delim, StringComparison.Ordinal));
        if (firstNewline < 0)
            return new ParsedDocument(FrontMatter.Default, document);

        var afterFirst = firstNewline + 1;
        var endDelim = document.IndexOf("\n" + Delim, afterFirst, StringComparison.Ordinal);
        if (endDelim < 0)
            return new ParsedDocument(FrontMatter.Default, document);

        var yaml = document.Substring(afterFirst, endDelim - afterFirst);
        var bodyStart = endDelim + 1 + Delim.Length;
        if (bodyStart < document.Length && document[bodyStart] == '\n') bodyStart++;
        var body = bodyStart < document.Length ? document[bodyStart..] : string.Empty;

        var fm = string.IsNullOrWhiteSpace(yaml)
            ? FrontMatter.Default.WithTitle(string.Empty)
            : Deserializer.Deserialize<YamlFrontMatterDto>(yaml).ToFrontMatter();

        return new ParsedDocument(fm, body);
    }

    public string Render(FrontMatter frontMatter, string body)
    {
        ArgumentNullException.ThrowIfNull(frontMatter);
        ArgumentNullException.ThrowIfNull(body);

        var dto = new YamlFrontMatterDto
        {
            Title = frontMatter.Title,
            Date = frontMatter.Date,
            Tags = frontMatter.Tags.IsDefault ? [] : [.. frontMatter.Tags],
            Draft = frontMatter.Draft,
            Description = frontMatter.Description,
        };
        var yaml = Serializer.Serialize(dto).TrimEnd();
        return $"{Delim}\n{yaml}\n{Delim}\n\n{body.TrimStart('\r', '\n')}";
    }

    private sealed class YamlFrontMatterDto
    {
        public string Title { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public List<string> Tags { get; set; } = [];
        public bool Draft { get; set; } = true;
        public string? Description { get; set; }

        public FrontMatter ToFrontMatter() =>
            new(Title, Date, [.. Tags ?? []], Draft, Description);
    }
}
