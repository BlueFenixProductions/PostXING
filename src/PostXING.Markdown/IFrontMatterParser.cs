using PostXING.Core.Domain;

namespace PostXING.Markdown;

public sealed record ParsedDocument(FrontMatter FrontMatter, string Body);

public interface IFrontMatterParser
{
    ParsedDocument Parse(string document);
    string Render(FrontMatter frontMatter, string body);
}
