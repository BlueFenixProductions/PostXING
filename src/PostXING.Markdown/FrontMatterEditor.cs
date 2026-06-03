namespace PostXING.Markdown;

/// <summary>Surgical edits to a document's YAML front matter that preserve the author's exact
/// formatting (key order, comments, spacing) — used instead of a parse+reserialize, which would
/// rewrite the whole block.</summary>
public static class FrontMatterEditor
{
    /// <summary>Return <paramref name="raw"/> with the front matter's <c>draft:</c> value set to
    /// <paramref name="draft"/>. If the document has a <c>---</c> fence but no <c>draft:</c> key,
    /// one is inserted; if it has no fence at all, the text is returned unchanged.</summary>
    public static string WithDraft(string raw, bool draft)
    {
        ArgumentNullException.ThrowIfNull(raw);

        var newline = raw.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var lines = raw.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');

        var open = FindOpenFence(lines);
        if (open < 0) return raw;
        var close = FindCloseFence(lines, open);
        if (close < 0) return raw;

        var value = draft ? "true" : "false";
        for (var i = open + 1; i < close; i++)
        {
            var trimmed = lines[i].TrimStart();
            if (!trimmed.StartsWith("draft:", StringComparison.OrdinalIgnoreCase)) continue;
            var indent = lines[i][..(lines[i].Length - trimmed.Length)];
            lines[i] = $"{indent}draft: {value}";
            return string.Join(newline, lines);
        }

        // No draft key inside the fence — insert one just before the closing fence.
        var withInsert = new List<string>(lines);
        withInsert.Insert(close, $"draft: {value}");
        return string.Join(newline, withInsert);
    }

    private static int FindOpenFence(string[] lines)
    {
        for (var i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            return lines[i].Trim() == "---" ? i : -1;
        }
        return -1;
    }

    private static int FindCloseFence(string[] lines, int open)
    {
        for (var i = open + 1; i < lines.Length; i++)
            if (lines[i].Trim() == "---") return i;
        return -1;
    }
}
