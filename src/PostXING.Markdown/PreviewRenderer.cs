using System.Net;
using System.Text;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using YamlDotNet.RepresentationModel;

namespace PostXING.Markdown;

/// <summary>
/// Builds the preview WebView document as a GitHub-README render: the markdown body wrapped in
/// <c>.markdown-body</c> (styled by the bundled github-markdown CSS, colorblind dark/light) with the
/// frontmatter shown as a GitHub-native table at the top. Pure; the caller supplies the CSS text + theme.
/// </summary>
public sealed class PreviewRenderer
{
    private const string Delim = "---";

    // HTML enabled so authored inline HTML renders the way it would on GitHub.
    private readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
        .UsePipeTables()
        .UseTaskLists()
        .UseEmphasisExtras()
        .UseFootnotes()
        .UseGenericAttributes()
        .Build();

    /// <summary>Back-compat brightness overload (GitHub light/dark canvas). Delegates to the
    /// palette overload with the GitHub colorblind canvas/fg/accent/border for that brightness.</summary>
    public string Build(string markdown, string githubCss, bool dark = false) =>
        Build(markdown, githubCss,
            canvas: dark ? "#0d1117" : "#ffffff",
            fg: dark ? "#f0f6fc" : "#1f2328",
            accent: dark ? "#1f6feb" : "#0969da",
            link: dark ? "#1f6feb" : "#0969da",
            codeBg: dark ? "#161b22" : "#f6f8fa",
            border: dark ? "#30363d" : "#d0d7de");

    /// <summary>Render the preview with an explicit theme palette layered over the bundled github CSS,
    /// so any of the gallery themes color the canvas, text, links, code blocks, and borders. Markdown
    /// can't reference the ViewModels' PreviewPalette type, so the caller unpacks it into plain strings.</summary>
    public string Build(string markdown, string githubCss,
        string canvas, string fg, string accent, string link, string codeBg, string border)
    {
        var (frontmatter, body) = Split(markdown ?? string.Empty);
        var bodyHtml = Markdig.Markdown.ToHtml(body, _pipeline);
        var fmTable = BuildFrontmatterTable(frontmatter);

        var sb = new StringBuilder(bodyHtml.Length + (githubCss?.Length ?? 0) + 1024);
        sb.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\">");
        sb.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.Append("<style>").Append(githubCss ?? string.Empty).Append("</style>");
        // Theme overrides layered on top of the bundled github CSS so any palette renders. The
        // explicit .markdown-body + table-cell colors guard readability: in the WebView those cells
        // weren't inheriting the body fg, rendering dark-on-dark.
        sb.Append("<style>body{margin:0;background:").Append(canvas)
          .Append(";}.markdown-body{box-sizing:border-box;max-width:980px;margin:0 auto;padding:32px 40px;color:")
          .Append(fg).Append(";}")
          .Append(".markdown-body img{max-width:100%;height:auto;}")
          .Append(".markdown-body a{color:").Append(link).Append(";}")
          .Append(".markdown-body h1,.markdown-body h2{border-bottom-color:").Append(border).Append(";}")
          .Append(".markdown-body hr{background-color:").Append(border).Append(";}")
          .Append(".markdown-body blockquote{border-left-color:").Append(accent).Append(";}")
          .Append(".markdown-body code,.markdown-body pre{background-color:").Append(codeBg).Append(";}")
          .Append(".markdown-body table th,.markdown-body table td{color:").Append(fg)
          .Append(";border-color:").Append(border).Append(";}</style>");
        sb.Append("</head><body><article class=\"markdown-body\">");
        sb.Append(fmTable);
        sb.Append(bodyHtml);
        sb.Append("</article></body></html>");
        return sb.ToString();
    }

    private static (string frontmatter, string body) Split(string doc)
    {
        var span = doc.AsSpan().TrimStart();
        if (!span.StartsWith(Delim)) return (string.Empty, doc);

        var firstNewline = doc.IndexOf('\n', doc.IndexOf(Delim, StringComparison.Ordinal));
        if (firstNewline < 0) return (string.Empty, doc);
        var afterFirst = firstNewline + 1;
        var endDelim = doc.IndexOf("\n" + Delim, afterFirst, StringComparison.Ordinal);
        if (endDelim < 0) return (string.Empty, doc);

        var yaml = doc.Substring(afterFirst, endDelim - afterFirst);
        var bodyStart = endDelim + 1 + Delim.Length;
        if (bodyStart < doc.Length && doc[bodyStart] == '\n') bodyStart++;
        var body = bodyStart < doc.Length ? doc[bodyStart..] : string.Empty;
        return (yaml, body);
    }

    // Frontmatter as a plain HTML table inside .markdown-body so GitHub's own table styling
    // (borders + row striping) renders it. Parsed from the raw YAML to keep all keys, in order.
    private static string BuildFrontmatterTable(string yaml)
    {
        if (string.IsNullOrWhiteSpace(yaml)) return string.Empty;

        YamlMappingNode? root = null;
        try
        {
            var stream = new YamlStream();
            stream.Load(new StringReader(yaml));
            if (stream.Documents.Count > 0) root = stream.Documents[0].RootNode as YamlMappingNode;
        }
        catch (YamlDotNet.Core.YamlException)
        {
            return string.Empty;
        }
        if (root is null || root.Children.Count == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.Append("<table><tbody>");
        foreach (var pair in root.Children)
        {
            var key = (pair.Key as YamlScalarNode)?.Value ?? pair.Key.ToString();
            // Single key|value row like github.com: keys as <th>, the value (incl. lists) in one <td>.
            sb.Append("<tr><th>").Append(WebUtility.HtmlEncode(key)).Append("</th><td>")
              .Append(RenderYamlValue(pair.Value)).Append("</td></tr>");
        }
        sb.Append("</tbody></table>");
        return sb.ToString();
    }

    private static string RenderYamlValue(YamlNode node)
    {
        switch (node)
        {
            case YamlScalarNode scalar:
                return WebUtility.HtmlEncode(scalar.Value ?? string.Empty);

            case YamlSequenceNode seq when seq.Children.All(c => c is YamlScalarNode):
                // GitHub shows a frontmatter scalar list space-separated within the single value cell.
                return string.Join(" ", seq.Children.Select(c => WebUtility.HtmlEncode(((YamlScalarNode)c).Value ?? string.Empty)));

            case YamlSequenceNode seq:
                var list = new StringBuilder("<ul>");
                foreach (var item in seq.Children) list.Append("<li>").Append(RenderYamlValue(item)).Append("</li>");
                return list.Append("</ul>").ToString();

            case YamlMappingNode map:
                var nested = new StringBuilder();
                foreach (var pair in map.Children)
                    nested.Append("<div><strong>")
                          .Append(WebUtility.HtmlEncode((pair.Key as YamlScalarNode)?.Value ?? string.Empty))
                          .Append(":</strong> ").Append(RenderYamlValue(pair.Value)).Append("</div>");
                return nested.ToString();

            default:
                return WebUtility.HtmlEncode(node.ToString() ?? string.Empty);
        }
    }
}
