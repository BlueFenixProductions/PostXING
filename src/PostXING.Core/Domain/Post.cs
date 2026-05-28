namespace PostXING.Core.Domain;

public sealed record Post(
    Slug Slug,
    FrontMatter FrontMatter,
    string Content,
    DateOnly CreatedAt)
{
    public string RelativePath(string contentRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentRoot);
        return $"{contentRoot.TrimEnd('/', '\\')}/{CreatedAt:yyyy}/{CreatedAt:MM}/{Slug.Value}.md";
    }

    public Post WithContent(string content) => this with { Content = content };
    public Post WithFrontMatter(FrontMatter fm) => this with { FrontMatter = fm };
}
