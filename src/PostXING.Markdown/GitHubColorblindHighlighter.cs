using System.Text;
using System.Text.RegularExpressions;

namespace PostXING.Markdown;

/// <summary>
/// Markdown + YAML frontmatter + embedded-CSS tokenizer that renders HTML spans keyed for the
/// GitHub Dark Colorblind / Light Colorblind theme that the editor WebView styles.
/// Pure, allocation-friendly, fully covered by GitHubColorblindHighlighterTests.
///
/// Class vocabulary (the editor CSS must match these):
///   yk  yaml key            yp  yaml/punctuation separator
///   ys  yaml string value   yn  yaml number/bool value
///   md-h  heading text      md-hm  heading marker (#)
///   md-em em marker (** _)  md-bold bold text  md-italic italic text
///   md-li list marker       md-code inline code  md-codeblock fenced code line
///   md-link-text [...]      md-link-url (...)  md-link-pct punctuation [ ] ( )
///   md-bq  blockquote line  md-hr horizontal rule
///   md-html html comment block
///   html  html tag (e.g. &lt;style&gt;, &lt;script&gt;, &lt;br/&gt;, &lt;VPTeamMembers/&gt;)
///   html-attr html attribute name    html-str html attribute value
///   css-sel  css selector   css-at  css at-rule (@media)
///   css-prop css property    css-val css value
///   css-punct css { } : ;
///   js-kw  js keyword       js-str  js string literal
///   js-num js number        js-comment js // and /* */ comment
/// </summary>
public sealed class GitHubColorblindHighlighter : IMarkdownHighlighter
{
    public string HighlightToHtml(string markdown)
    {
        if (string.IsNullOrEmpty(markdown)) return string.Empty;

        var lines = markdown.Split('\n');
        var out_ = new StringBuilder(markdown.Length + 64);
        var inFm = false;
        var fmConsumed = false;
        var inFence = false;
        var inStyle = false;
        var inScript = false;
        var jsBlockComment = false;

        for (var k = 0; k < lines.Length; k++)
        {
            var line = lines[k];
            var trimmed = line.TrimEnd();

            // YAML frontmatter delimiter: only the first --- at top of file opens, paired ---
            // closes. A --- anywhere else is a horizontal rule.
            if (trimmed == "---" && !inFence && !inStyle && !inScript)
            {
                if (!fmConsumed && k == 0)
                {
                    inFm = true;
                    fmConsumed = true;
                    out_.Append("<span class=\"yp\">---</span>");
                    if (k < lines.Length - 1) out_.Append('\n');
                    continue;
                }
                if (inFm)
                {
                    inFm = false;
                    out_.Append("<span class=\"yp\">---</span>");
                    if (k < lines.Length - 1) out_.Append('\n');
                    continue;
                }
            }

            if (inFm)
            {
                out_.Append(YamlLine(line));
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            // Code fence toggle (entire line including the fence is classed as codeblock)
            if (!inStyle && !inScript && Regex.IsMatch(line.Trim(), "^```"))
            {
                out_.Append("<span class=\"md-codeblock\">").Append(Escape(line)).Append("</span>");
                inFence = !inFence;
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            // <style> open: tag on the line, switch into CSS mode for subsequent lines.
            if (!inFence && !inStyle && !inScript && Regex.IsMatch(line, @"<style\b[^>]*>", RegexOptions.IgnoreCase))
            {
                out_.Append(RenderStyleOpenLine(line));
                inStyle = true;
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            // </style> close: tag on the line, leave CSS mode.
            if (inStyle && Regex.IsMatch(line, @"</style\s*>", RegexOptions.IgnoreCase))
            {
                out_.Append(RenderStyleCloseLine(line));
                inStyle = false;
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            if (inStyle)
            {
                out_.Append(CssLine(line));
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            // <script> open: tag on the line, switch into JS mode for subsequent lines.
            if (!inFence && !inScript && Regex.IsMatch(line, @"<script\b[^>]*>", RegexOptions.IgnoreCase))
            {
                out_.Append(RenderScriptOpenLine(line, ref jsBlockComment));
                inScript = true;
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            // </script> close: tag on the line, leave JS mode.
            if (inScript && Regex.IsMatch(line, @"</script\s*>", RegexOptions.IgnoreCase))
            {
                out_.Append(RenderScriptCloseLine(line, ref jsBlockComment));
                inScript = false;
                jsBlockComment = false;
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            if (inScript)
            {
                out_.Append(JsLine(line, ref jsBlockComment));
                if (k < lines.Length - 1) out_.Append('\n');
                continue;
            }

            out_.Append(MdLine(line, inFence));
            if (k < lines.Length - 1) out_.Append('\n');
        }

        return out_.ToString();
    }

    private static string YamlLine(string line)
    {
        // Variant A: list item `- key: value` or `- value`
        var li = Regex.Match(line, @"^(\s*)(-\s+)(.*)$");
        if (li.Success)
        {
            var indent = li.Groups[1].Value;
            var marker = li.Groups[2].Value;
            var rest = li.Groups[3].Value;

            var sb = new StringBuilder();
            sb.Append(Escape(indent));
            sb.Append("<span class=\"yp\">").Append(Escape(marker)).Append("</span>");

            // The rest can be a key:value pair or a bare scalar.
            var kv = Regex.Match(rest, @"^([A-Za-z_][\w\-.]*)(\s*:)(\s*)(.*)$");
            if (kv.Success)
            {
                AppendYamlKeyValue(sb, kv.Groups[1].Value, kv.Groups[2].Value, kv.Groups[3].Value, kv.Groups[4].Value);
            }
            else if (rest.Length > 0)
            {
                sb.Append("<span class=\"").Append(ClassifyYamlValue(rest)).Append("\">").Append(Escape(rest)).Append("</span>");
            }
            return sb.ToString();
        }

        // Variant B: plain `key: value`
        var m = Regex.Match(line, @"^(\s*)([A-Za-z_][\w\-.]*)(\s*:)(\s*)(.*)$");
        if (!m.Success) return Escape(line);

        var sb2 = new StringBuilder();
        sb2.Append(Escape(m.Groups[1].Value));
        AppendYamlKeyValue(sb2, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value, m.Groups[5].Value);
        return sb2.ToString();
    }

    private static void AppendYamlKeyValue(StringBuilder sb, string key, string colon, string sp, string value)
    {
        sb.Append("<span class=\"yk\">").Append(Escape(key)).Append("</span>");
        sb.Append("<span class=\"yp\">").Append(Escape(colon)).Append("</span>");
        sb.Append(Escape(sp));
        if (value.Length > 0)
            sb.Append("<span class=\"").Append(ClassifyYamlValue(value)).Append("\">").Append(Escape(value)).Append("</span>");
    }

    private static string ClassifyYamlValue(string value)
    {
        var trimmed = value.Trim();
        if (trimmed.Length == 0 || trimmed == "[]" || trimmed == "{}"
            || trimmed.StartsWith('[')
            || trimmed.StartsWith('{'))
        {
            return "yp";
        }
        if (Regex.IsMatch(trimmed, @"^(true|false|null|~|yes|no)$", RegexOptions.IgnoreCase)) return "yn";
        if (Regex.IsMatch(trimmed, @"^-?\d+(\.\d+)?$")) return "yn";
        return "ys";
    }

    private static string RenderStyleOpenLine(string line)
    {
        // Match the <style ...> tag and class it; anything after on the same line is treated as CSS.
        var m = Regex.Match(line, @"^(.*?)(<style\b[^>]*>)(.*)$", RegexOptions.IgnoreCase);
        if (!m.Success) return Escape(line);

        var sb = new StringBuilder();
        sb.Append(MdLine(m.Groups[1].Value, inFence: false));
        sb.Append("<span class=\"html\">").Append(Escape(m.Groups[2].Value)).Append("</span>");
        var tail = m.Groups[3].Value;
        if (tail.Length > 0) sb.Append(CssLine(tail));
        return sb.ToString();
    }

    private static string RenderStyleCloseLine(string line)
    {
        var m = Regex.Match(line, @"^(.*?)(</style\s*>)(.*)$", RegexOptions.IgnoreCase);
        if (!m.Success) return Escape(line);

        var sb = new StringBuilder();
        var head = m.Groups[1].Value;
        if (head.Length > 0) sb.Append(CssLine(head));
        sb.Append("<span class=\"html\">").Append(Escape(m.Groups[2].Value)).Append("</span>");
        var tail = m.Groups[3].Value;
        if (tail.Length > 0) sb.Append(MdLine(tail, inFence: false));
        return sb.ToString();
    }

    // CSS line tokenizer. Handles the shapes that show up in real-world Markdown
    // <style> blocks: selectors + braces, properties + values + semicolons, @-rules.
    // Doesn't attempt full CSS grammar — block-and-line oriented is enough for live editing.
    private static string CssLine(string line)
    {
        // Empty or whitespace-only: just escape and move on.
        if (string.IsNullOrWhiteSpace(line)) return Escape(line);

        // Standalone closing brace on a line (with optional whitespace).
        if (Regex.IsMatch(line, @"^\s*\}\s*$"))
        {
            return RenderCssWithPunct(line);
        }

        // Selector line: contains a `{` but no `:`-style declaration before it.
        var braceIdx = line.IndexOf('{');
        if (braceIdx >= 0)
        {
            // Selector portion + `{` + tail (could be inline rule on the same line)
            var head = line.Substring(0, braceIdx);
            var tail = line.Substring(braceIdx + 1);
            var sb = new StringBuilder();
            sb.Append(RenderCssSelector(head));
            sb.Append("<span class=\"css-punct\">{</span>");
            if (tail.Length > 0)
            {
                // Tail can be one or more declarations and possibly a closing brace.
                sb.Append(RenderCssDeclarations(tail));
            }
            return sb.ToString();
        }

        // Declaration line: `property: value;`
        var decl = Regex.Match(line, @"^(\s*)([A-Za-z_-][\w-]*)(\s*:\s*)(.*?)(\s*;?\s*)$");
        if (decl.Success && decl.Groups[2].Value.Length > 0)
        {
            var sb = new StringBuilder();
            sb.Append(Escape(decl.Groups[1].Value));
            sb.Append("<span class=\"css-prop\">").Append(Escape(decl.Groups[2].Value)).Append("</span>");
            // The colon (and any surrounding spaces): render the `:` as punct, keep spaces literal.
            var sep = decl.Groups[3].Value;
            AppendCssSepWithColon(sb, sep);
            sb.Append(RenderCssValue(decl.Groups[4].Value));
            var endRun = decl.Groups[5].Value;
            AppendCssEndRun(sb, endRun);
            return sb.ToString();
        }

        return Escape(line);
    }

    private static string RenderCssSelector(string head)
    {
        // Class an @-rule prefix if present.
        var at = Regex.Match(head, @"^(\s*)(@[A-Za-z-]+)(.*)$");
        if (at.Success)
        {
            var sb = new StringBuilder();
            sb.Append(Escape(at.Groups[1].Value));
            sb.Append("<span class=\"css-at\">").Append(Escape(at.Groups[2].Value)).Append("</span>");
            var rest = at.Groups[3].Value;
            if (rest.Length > 0)
            {
                sb.Append("<span class=\"css-sel\">").Append(Escape(rest)).Append("</span>");
            }
            return sb.ToString();
        }

        // Preserve leading indent literally, class only the selector substance.
        var lead = Regex.Match(head, @"^(\s*)(.*?)(\s*)$");
        if (!lead.Success || lead.Groups[2].Value.Length == 0) return Escape(head);

        var sb2 = new StringBuilder();
        sb2.Append(Escape(lead.Groups[1].Value));
        sb2.Append("<span class=\"css-sel\">").Append(Escape(lead.Groups[2].Value)).Append("</span>");
        sb2.Append(Escape(lead.Groups[3].Value));
        return sb2.ToString();
    }

    private static string RenderCssDeclarations(string tail)
    {
        // Split on `;` but keep the separators in the output.
        var sb = new StringBuilder();
        var parts = tail.Split(';');
        for (var i = 0; i < parts.Length; i++)
        {
            var seg = parts[i];
            // A close brace on the tail terminates declarations.
            var closeIdx = seg.IndexOf('}');
            if (closeIdx >= 0)
            {
                var before = seg.Substring(0, closeIdx);
                var after = seg.Substring(closeIdx + 1);
                if (before.Trim().Length > 0) sb.Append(CssLine(before));
                else sb.Append(Escape(before));
                sb.Append("<span class=\"css-punct\">}</span>");
                if (after.Length > 0) sb.Append(Escape(after));
                if (i < parts.Length - 1) sb.Append("<span class=\"css-punct\">;</span>");
                continue;
            }
            if (seg.Trim().Length == 0) sb.Append(Escape(seg));
            else sb.Append(CssLine(seg));
            if (i < parts.Length - 1) sb.Append("<span class=\"css-punct\">;</span>");
        }
        return sb.ToString();
    }

    private static void AppendCssSepWithColon(StringBuilder sb, string sep)
    {
        var idx = sep.IndexOf(':');
        if (idx < 0) { sb.Append(Escape(sep)); return; }
        sb.Append(Escape(sep.Substring(0, idx)));
        sb.Append("<span class=\"css-punct\">:</span>");
        sb.Append(Escape(sep.Substring(idx + 1)));
    }

    private static void AppendCssEndRun(StringBuilder sb, string endRun)
    {
        var idx = endRun.IndexOf(';');
        if (idx < 0) { sb.Append(Escape(endRun)); return; }
        sb.Append(Escape(endRun.Substring(0, idx)));
        sb.Append("<span class=\"css-punct\">;</span>");
        sb.Append(Escape(endRun.Substring(idx + 1)));
    }

    private static string RenderCssValue(string value)
    {
        if (value.Length == 0) return string.Empty;
        return "<span class=\"css-val\">" + Escape(value) + "</span>";
    }

    private static string RenderCssWithPunct(string line)
    {
        var sb = new StringBuilder();
        foreach (var ch in line)
        {
            if (ch == '{' || ch == '}') sb.Append("<span class=\"css-punct\">").Append(ch).Append("</span>");
            else Escape(sb, ch);
        }
        return sb.ToString();
    }

    private static string RenderScriptOpenLine(string line, ref bool inBlockComment)
    {
        var m = Regex.Match(line, @"^(.*?)(<script\b[^>]*>)(.*)$", RegexOptions.IgnoreCase);
        if (!m.Success) return Escape(line);

        var sb = new StringBuilder();
        sb.Append(MdLine(m.Groups[1].Value, inFence: false));
        sb.Append("<span class=\"html\">").Append(Escape(m.Groups[2].Value)).Append("</span>");
        var tail = m.Groups[3].Value;
        if (tail.Length > 0) sb.Append(JsLine(tail, ref inBlockComment));
        return sb.ToString();
    }

    private static string RenderScriptCloseLine(string line, ref bool inBlockComment)
    {
        var m = Regex.Match(line, @"^(.*?)(</script\s*>)(.*)$", RegexOptions.IgnoreCase);
        if (!m.Success) return Escape(line);

        var sb = new StringBuilder();
        var head = m.Groups[1].Value;
        if (head.Length > 0) sb.Append(JsLine(head, ref inBlockComment));
        sb.Append("<span class=\"html\">").Append(Escape(m.Groups[2].Value)).Append("</span>");
        var tail = m.Groups[3].Value;
        if (tail.Length > 0) sb.Append(MdLine(tail, inFence: false));
        return sb.ToString();
    }

    // Small set of JS-ish keywords — enough to feel correct in live editing without
    // pretending to be a real parser. Order here is irrelevant; lookup is by hash.
    private static readonly HashSet<string> JsKeywords = new(StringComparer.Ordinal)
    {
        "abstract","as","async","await","break","case","catch","class","const","continue",
        "debugger","default","delete","do","else","enum","export","extends","false","finally",
        "for","from","function","get","if","import","in","instanceof","interface","let","new",
        "null","of","package","private","protected","public","return","set","static","super",
        "switch","this","throw","true","try","type","typeof","undefined","var","void","while",
        "with","yield"
    };

    private static string JsLine(string line, ref bool inBlockComment)
    {
        var sb = new StringBuilder();
        var i = 0;
        var n = line.Length;

        while (i < n)
        {
            // Already inside a multi-line block comment: keep consuming until */ or end of line.
            if (inBlockComment)
            {
                var end = line.IndexOf("*/", i, StringComparison.Ordinal);
                if (end < 0)
                {
                    sb.Append("<span class=\"js-comment\">").Append(Escape(line.Substring(i))).Append("</span>");
                    return sb.ToString();
                }
                sb.Append("<span class=\"js-comment\">").Append(Escape(line.Substring(i, end + 2 - i))).Append("</span>");
                i = end + 2;
                inBlockComment = false;
                continue;
            }

            var c = line[i];
            var c2 = i + 1 < n ? line[i + 1] : '\0';

            // Line comment
            if (c == '/' && c2 == '/')
            {
                sb.Append("<span class=\"js-comment\">").Append(Escape(line.Substring(i))).Append("</span>");
                return sb.ToString();
            }

            // Block comment
            if (c == '/' && c2 == '*')
            {
                var end = line.IndexOf("*/", i + 2, StringComparison.Ordinal);
                if (end < 0)
                {
                    sb.Append("<span class=\"js-comment\">").Append(Escape(line.Substring(i))).Append("</span>");
                    inBlockComment = true;
                    return sb.ToString();
                }
                sb.Append("<span class=\"js-comment\">").Append(Escape(line.Substring(i, end + 2 - i))).Append("</span>");
                i = end + 2;
                continue;
            }

            // String literal: ', ", `
            if (c == '\'' || c == '"' || c == '`')
            {
                var end = FindStringEnd(line, i, c);
                if (end > i)
                {
                    sb.Append("<span class=\"js-str\">").Append(Escape(line.Substring(i, end + 1 - i))).Append("</span>");
                    i = end + 1;
                    continue;
                }
                // Unterminated string: emit what we have and move on.
                sb.Append("<span class=\"js-str\">").Append(Escape(line.Substring(i))).Append("</span>");
                return sb.ToString();
            }

            // Number literal: 42, 3.14, .5
            if (char.IsDigit(c) || (c == '.' && c2 != '\0' && char.IsDigit(c2)))
            {
                var start = i;
                while (i < n && (char.IsDigit(line[i]) || line[i] == '.')) i++;
                sb.Append("<span class=\"js-num\">").Append(Escape(line.Substring(start, i - start))).Append("</span>");
                continue;
            }

            // Identifier / keyword
            if (char.IsLetter(c) || c == '_' || c == '$')
            {
                var start = i;
                while (i < n && (char.IsLetterOrDigit(line[i]) || line[i] == '_' || line[i] == '$')) i++;
                var word = line.Substring(start, i - start);
                if (JsKeywords.Contains(word))
                    sb.Append("<span class=\"js-kw\">").Append(Escape(word)).Append("</span>");
                else
                    sb.Append(Escape(word));
                continue;
            }

            Escape(sb, c);
            i++;
        }
        return sb.ToString();
    }

    private static int FindStringEnd(string line, int start, char quote)
    {
        for (var i = start + 1; i < line.Length; i++)
        {
            var c = line[i];
            if (c == '\\') { i++; continue; }
            if (c == quote) return i;
        }
        return -1;
    }

    // Inline HTML / Vue / VitePress tag. Returns the number of characters consumed,
    // or 0 if the slice at `start` doesn't look like a tag.
    private static int RenderInlineHtmlTag(StringBuilder sb, string text, int start)
    {
        var end = text.IndexOf('>', start + 1);
        if (end < 0) return 0;

        var inner = text.Substring(start + 1, end - start - 1);
        var isClose = inner.StartsWith('/');
        if (isClose) inner = inner.Substring(1);

        // Tag name must start with a letter; allow letters, digits, '_', '-', '.', ':'.
        if (inner.Length == 0 || !char.IsLetter(inner[0])) return 0;
        var nameEnd = 1;
        while (nameEnd < inner.Length && (char.IsLetterOrDigit(inner[nameEnd]) || inner[nameEnd] == '_' || inner[nameEnd] == '-' || inner[nameEnd] == '.' || inner[nameEnd] == ':'))
            nameEnd++;
        var tagName = inner.Substring(0, nameEnd);
        var afterName = inner.Substring(nameEnd);

        var selfClose = afterName.TrimEnd().EndsWith('/');
        var attrs = selfClose ? afterName.Substring(0, afterName.LastIndexOf('/')) : afterName;

        sb.Append("<span class=\"html\">&lt;");
        if (isClose) sb.Append('/');
        sb.Append(Escape(tagName));
        sb.Append("</span>");

        if (attrs.Length > 0) sb.Append(RenderHtmlAttributes(attrs));

        sb.Append("<span class=\"html\">");
        if (selfClose) sb.Append('/');
        sb.Append("&gt;</span>");

        return end - start + 1;
    }

    private static string RenderHtmlAttributes(string attrs)
    {
        var sb = new StringBuilder();
        var i = 0;
        var n = attrs.Length;
        while (i < n)
        {
            var c = attrs[i];

            // Whitespace: pass through literally.
            if (char.IsWhiteSpace(c)) { sb.Append(c); i++; continue; }

            // Attribute name: letters, plus Vue-flavored leading punctuation (':', '@', '#', 'v-').
            if (char.IsLetter(c) || c == '_' || c == ':' || c == '@' || c == '#')
            {
                var nameStart = i;
                while (i < n && (char.IsLetterOrDigit(attrs[i]) || attrs[i] == '_' || attrs[i] == '-' || attrs[i] == ':' || attrs[i] == '@' || attrs[i] == '#' || attrs[i] == '.'))
                    i++;
                sb.Append("<span class=\"html-attr\">")
                  .Append(Escape(attrs.Substring(nameStart, i - nameStart)))
                  .Append("</span>");

                if (i < n && attrs[i] == '=')
                {
                    sb.Append('=');
                    i++;
                    if (i < n && (attrs[i] == '"' || attrs[i] == '\''))
                    {
                        var quote = attrs[i];
                        var valStart = i;
                        i++;
                        while (i < n && attrs[i] != quote) i++;
                        if (i < n) i++; // consume closing quote
                        sb.Append("<span class=\"html-str\">")
                          .Append(Escape(attrs.Substring(valStart, i - valStart)))
                          .Append("</span>");
                    }
                }
                continue;
            }

            Escape(sb, c);
            i++;
        }
        return sb.ToString();
    }

    private static string MdLine(string line, bool inFence)
    {
        if (inFence)
            return "<span class=\"md-codeblock\">" + Escape(line) + "</span>";

        // Horizontal rule: three or more *, -, or _ (possibly spaced)
        if (Regex.IsMatch(line, @"^\s*(\*\s*){3,}\s*$")
            || Regex.IsMatch(line, @"^\s*(-\s*){3,}\s*$")
            || Regex.IsMatch(line, @"^\s*(_\s*){3,}\s*$"))
        {
            return "<span class=\"md-hr\">" + Escape(line) + "</span>";
        }

        // ATX heading
        var hm = Regex.Match(line, @"^(\#{1,6})(\s+)(.*)$");
        if (hm.Success)
        {
            return "<span class=\"md-hm\">" + Escape(hm.Groups[1].Value) + "</span>"
                + Escape(hm.Groups[2].Value)
                + "<span class=\"md-h\">" + InlineMd(hm.Groups[3].Value) + "</span>";
        }

        // Blockquote
        if (Regex.IsMatch(line, @"^\s*>"))
        {
            return "<span class=\"md-bq\">" + Escape(line) + "</span>";
        }

        // List item
        var lm = Regex.Match(line, @"^(\s*)([-*+]|\d+\.)(\s+)(.*)$");
        if (lm.Success)
        {
            return Escape(lm.Groups[1].Value)
                + "<span class=\"md-li\">" + Escape(lm.Groups[2].Value) + "</span>"
                + Escape(lm.Groups[3].Value)
                + InlineMd(lm.Groups[4].Value);
        }

        return InlineMd(line);
    }

    private static string InlineMd(string text)
    {
        var sb = new StringBuilder();
        var i = 0;
        var n = text.Length;

        while (i < n)
        {
            var c = text[i];
            var c2 = i + 1 < n ? text[i + 1] : '\0';

            // HTML comment: <!-- ... -->
            if (c == '<' && i + 4 <= n && text.AsSpan(i, 4).SequenceEqual("<!--"))
            {
                var end = text.IndexOf("-->", i + 4, StringComparison.Ordinal);
                if (end > i)
                {
                    sb.Append("<span class=\"md-html\">")
                      .Append(Escape(text.Substring(i, end + 3 - i)))
                      .Append("</span>");
                    i = end + 3;
                    continue;
                }
            }

            // Inline HTML/Vue tag: <Tag ...>, </Tag>, <Tag .../>
            if (c == '<' && i + 1 < n && (text[i + 1] == '/' || char.IsLetter(text[i + 1])))
            {
                var consumed = RenderInlineHtmlTag(sb, text, i);
                if (consumed > 0)
                {
                    i += consumed;
                    continue;
                }
            }

            // Inline code: `...`
            if (c == '`')
            {
                var end = text.IndexOf('`', i + 1);
                if (end > i)
                {
                    sb.Append("<span class=\"md-code\">")
                      .Append(Escape(text.Substring(i, end + 1 - i)))
                      .Append("</span>");
                    i = end + 1;
                    continue;
                }
            }

            // Bold: **...**
            if (c == '*' && c2 == '*')
            {
                var end = text.IndexOf("**", i + 2, StringComparison.Ordinal);
                if (end > i + 1)
                {
                    sb.Append("<span class=\"md-em\">**</span>")
                      .Append("<span class=\"md-bold\">").Append(InlineMd(text.Substring(i + 2, end - (i + 2)))).Append("</span>")
                      .Append("<span class=\"md-em\">**</span>");
                    i = end + 2;
                    continue;
                }
            }

            // Italic: *...* or _..._
            if ((c == '*' || c == '_') && c2 != ' ' && c2 != c && c2 != '\0')
            {
                var end = text.IndexOf(c, i + 1);
                if (end > i + 1 && text[end - 1] != ' ')
                {
                    sb.Append("<span class=\"md-em\">").Append(c).Append("</span>")
                      .Append("<span class=\"md-italic\">").Append(InlineMd(text.Substring(i + 1, end - (i + 1)))).Append("</span>")
                      .Append("<span class=\"md-em\">").Append(c).Append("</span>");
                    i = end + 1;
                    continue;
                }
            }

            // Image: ![alt](url)
            if (c == '!' && c2 == '[')
            {
                var labelEnd = text.IndexOf(']', i + 2);
                if (labelEnd > 0 && labelEnd + 1 < n && text[labelEnd + 1] == '(')
                {
                    var urlEnd = text.IndexOf(')', labelEnd + 2);
                    if (urlEnd > 0)
                    {
                        sb.Append("<span class=\"md-link-pct\">![</span>")
                          .Append("<span class=\"md-link-text\">").Append(Escape(text.Substring(i + 2, labelEnd - (i + 2)))).Append("</span>")
                          .Append("<span class=\"md-link-pct\">](</span>")
                          .Append("<span class=\"md-link-url\">").Append(Escape(text.Substring(labelEnd + 2, urlEnd - (labelEnd + 2)))).Append("</span>")
                          .Append("<span class=\"md-link-pct\">)</span>");
                        i = urlEnd + 1;
                        continue;
                    }
                }
            }

            // Link: [text](url)
            if (c == '[')
            {
                var labelEnd = text.IndexOf(']', i + 1);
                if (labelEnd > 0 && labelEnd + 1 < n && text[labelEnd + 1] == '(')
                {
                    var urlEnd = text.IndexOf(')', labelEnd + 2);
                    if (urlEnd > 0)
                    {
                        sb.Append("<span class=\"md-link-pct\">[</span>")
                          .Append("<span class=\"md-link-text\">").Append(Escape(text.Substring(i + 1, labelEnd - (i + 1)))).Append("</span>")
                          .Append("<span class=\"md-link-pct\">](</span>")
                          .Append("<span class=\"md-link-url\">").Append(Escape(text.Substring(labelEnd + 2, urlEnd - (labelEnd + 2)))).Append("</span>")
                          .Append("<span class=\"md-link-pct\">)</span>");
                        i = urlEnd + 1;
                        continue;
                    }
                }
            }

            sb.Append(Escape(c));
            i++;
        }
        return sb.ToString();
    }

    private static string Escape(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        var sb = new StringBuilder(s.Length);
        foreach (var c in s) Escape(sb, c);
        return sb.ToString();
    }

    private static string Escape(char c)
    {
        return c switch
        {
            '&' => "&amp;",
            '<' => "&lt;",
            '>' => "&gt;",
            _ => c.ToString(),
        };
    }

    private static void Escape(StringBuilder sb, char c)
    {
        switch (c)
        {
            case '&': sb.Append("&amp;"); break;
            case '<': sb.Append("&lt;"); break;
            case '>': sb.Append("&gt;"); break;
            default: sb.Append(c); break;
        }
    }
}
