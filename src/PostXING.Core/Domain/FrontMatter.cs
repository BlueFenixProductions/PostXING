using System.Collections.Immutable;

namespace PostXING.Core.Domain;

public sealed class FrontMatter : IEquatable<FrontMatter>
{
    public string Title { get; }
    public DateOnly Date { get; }
    public ImmutableArray<string> Tags { get; }
    public bool Draft { get; }
    public string? Description { get; }

    public FrontMatter(string? title, DateOnly date, ImmutableArray<string> tags, bool draft, string? description)
    {
        Title = title ?? string.Empty;
        Date = date;
        Tags = tags.IsDefault ? [] : tags;
        Draft = draft;
        Description = description;
    }

    public static FrontMatter Default { get; } = new(
        title: string.Empty,
        date: DateOnly.FromDateTime(DateTime.UtcNow),
        tags: [],
        draft: true,
        description: null);

    public FrontMatter WithTitle(string title) =>
        new(title, Date, Tags, Draft, Description);

    public FrontMatter WithDate(DateOnly date) =>
        new(Title, date, Tags, Draft, Description);

    public FrontMatter WithDraft(bool draft) =>
        new(Title, Date, Tags, draft, Description);

    public FrontMatter WithDescription(string? description) =>
        new(Title, Date, Tags, Draft, description);

    public FrontMatter WithTag(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        if (Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)) return this;
        return new FrontMatter(Title, Date, Tags.Add(tag.Trim()), Draft, Description);
    }

    public FrontMatter WithoutTag(string tag) =>
        new(Title, Date, Tags.RemoveAll(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase)), Draft, Description);

    public bool Equals(FrontMatter? other) =>
        other is not null
        && Title == other.Title
        && Date == other.Date
        && Draft == other.Draft
        && Description == other.Description
        && Tags.SequenceEqual(other.Tags, StringComparer.Ordinal);

    public override bool Equals(object? obj) => obj is FrontMatter fm && Equals(fm);

    public override int GetHashCode() => HashCode.Combine(Title, Date, Draft, Description, Tags.Length);
}
