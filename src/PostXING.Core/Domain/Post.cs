namespace PostXING.Core.Domain;

public sealed record Post(
    Slug Slug,
    FrontMatter FrontMatter,
    string Content,
    DateOnly CreatedAt)
{
    public Post WithContent(string content) => this with { Content = content };
    public Post WithFrontMatter(FrontMatter fm) => this with { FrontMatter = fm };
}
