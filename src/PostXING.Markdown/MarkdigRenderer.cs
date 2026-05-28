using Markdig;
using Markdig.Extensions.AutoIdentifiers;

namespace PostXING.Markdown;

public sealed class MarkdigRenderer : IMarkdownRenderer
{
    private readonly MarkdownPipeline _htmlPipeline = new MarkdownPipelineBuilder()
        .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
        .UsePipeTables()
        .UseTaskLists()
        .UseEmphasisExtras()
        .UseFootnotes()
        .UseGenericAttributes()
        .DisableHtml()
        .Build();

    private readonly MarkdownPipeline _plainPipeline = new MarkdownPipelineBuilder()
        .Build();

    public string RenderHtml(string markdown) =>
        Markdig.Markdown.ToHtml(markdown ?? string.Empty, _htmlPipeline);

    public string RenderPlainText(string markdown) =>
        Markdig.Markdown.ToPlainText(markdown ?? string.Empty, _plainPipeline);
}
