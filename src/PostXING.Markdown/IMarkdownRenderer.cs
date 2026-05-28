namespace PostXING.Markdown;

public interface IMarkdownRenderer
{
    string RenderHtml(string markdown);
    string RenderPlainText(string markdown);
}
