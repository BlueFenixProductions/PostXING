using System.Collections.Immutable;
using System.Globalization;
using PostXING.Core.Domain;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PostXING.Markdown;

public sealed class YamlFrontMatterParser : IFrontMatterParser
{
    private const string Delim = "---";

    // The VitePress blog theme keys its post layout off `layout: post`; emit it on every
    // rendered post so PostXING-written posts render like the legacy create-post.mjs ones (gh #44).
    private const string LayoutPost = "post";

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(LowerCaseNamingConvention.Instance)
        .WithTypeConverter(new DateOnlyYamlConverter())
        .IgnoreUnmatchedProperties()
        .Build();

    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(LowerCaseNamingConvention.Instance)
        .WithTypeConverter(new DateOnlyYamlConverter())
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
            Layout = LayoutPost,
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
        public string Layout { get; set; } = LayoutPost;
        public string Title { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public List<string> Tags { get; set; } = [];
        public bool Draft { get; set; } = true;
        public string? Description { get; set; }

        public FrontMatter ToFrontMatter() =>
            new(Title, Date, [.. Tags ?? []], Draft, Description);
    }

    // YamlDotNet's built-in DateOnly handling serializes the struct by decomposing it into its
    // public properties (year/month/day/dayofweek/...), which the VitePress theme can't read as a
    // date and which doesn't round-trip back through Parse. Emit/read a plain ISO yyyy-MM-dd string.
    private sealed class DateOnlyYamlConverter : IYamlTypeConverter
    {
        private const string Format = "yyyy-MM-dd";

        public bool Accepts(Type type) => type == typeof(DateOnly);

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            var value = parser.Consume<Scalar>().Value;
            if (DateOnly.TryParse(value, CultureInfo.InvariantCulture, out var date))
                return date;
            // Tolerate a full ISO datetime (e.g. a legacy post with a time component).
            return DateOnly.FromDateTime(DateTime.Parse(value, CultureInfo.InvariantCulture));
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            var date = (DateOnly)(value ?? default(DateOnly));
            emitter.Emit(new Scalar(date.ToString(Format, CultureInfo.InvariantCulture)));
        }
    }
}
