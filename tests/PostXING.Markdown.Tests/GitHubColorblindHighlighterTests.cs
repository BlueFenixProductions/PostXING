using PostXING.Markdown;
using Shouldly;
using Xunit;

namespace PostXING.Markdown.Tests;

/// <summary>
/// TDD spec for the live-editor syntax highlighter. Class lists are the load-bearing contract:
/// the editor CSS keys off them. If you rename a class here, update Resources/Raw/editor/index.html.
/// </summary>
public sealed class GitHubColorblindHighlighterTests
{
    private readonly GitHubColorblindHighlighter _sut = new();

    [Fact]
    public void Empty_input_returns_empty_string()
    {
        _sut.HighlightToHtml(string.Empty).ShouldBe(string.Empty);
    }

    [Fact]
    public void Plain_prose_returns_escaped_text_with_no_spans()
    {
        var result = _sut.HighlightToHtml("just some prose");
        result.ShouldBe("just some prose");
    }

    [Fact]
    public void Html_unsafe_characters_are_escaped()
    {
        var result = _sut.HighlightToHtml("a < b & c > d");
        result.ShouldContain("a &lt; b &amp; c &gt; d");
        result.ShouldNotContain("<b>");
    }

    [Fact]
    public void Heading_marker_and_text_are_classed()
    {
        var result = _sut.HighlightToHtml("# Hello");
        result.ShouldContain("class=\"md-hm\"");
        result.ShouldContain("#");
        result.ShouldContain("class=\"md-h\"");
        result.ShouldContain(">Hello<");
    }

    [Fact]
    public void Multilevel_headings_recognized_through_h6()
    {
        _sut.HighlightToHtml("###### h6").ShouldContain("######");
        _sut.HighlightToHtml("## h2").ShouldContain("##");
    }

    [Fact]
    public void Yaml_frontmatter_separator_uses_punctuation_class()
    {
        var input = "---\ntitle: Hello\n---\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yp\"");
        result.ShouldContain("---");
    }

    [Fact]
    public void Yaml_key_is_keyword_class_value_is_string_class()
    {
        var input = "---\ntitle: Hello World\n---\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yk\"");
        result.ShouldContain(">title<");
        result.ShouldContain("class=\"ys\"");
        result.ShouldContain(">Hello World<");
    }

    [Fact]
    public void Yaml_bool_value_is_number_class()
    {
        var input = "---\ndraft: true\n---\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yn\"");
        result.ShouldContain(">true<");
    }

    [Fact]
    public void Yaml_numeric_value_is_number_class()
    {
        var input = "---\norder: 42\n---\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yn\"");
        result.ShouldContain(">42<");
    }

    [Fact]
    public void Frontmatter_only_applies_when_it_starts_at_first_line()
    {
        // A `---` in the middle of a document is a horizontal rule, not a frontmatter open.
        var input = "some prose\n\n---\n\nmore prose";
        var result = _sut.HighlightToHtml(input);

        // The first --- should NOT be classed as a frontmatter separator; treat as HR.
        result.ShouldContain("class=\"md-hr\"");
    }

    [Fact]
    public void Frontmatter_closes_on_second_separator()
    {
        var input = "---\ntitle: A\n---\n# Heading after";
        var result = _sut.HighlightToHtml(input);

        // After the closing ---, `# Heading after` should be markdown heading, not yaml.
        result.ShouldContain("class=\"md-h\"");
        result.ShouldContain(">Heading after<");
    }

    [Fact]
    public void Unordered_list_marker_is_variable_class()
    {
        var result = _sut.HighlightToHtml("- first item");
        result.ShouldContain("class=\"md-li\"");
        result.ShouldContain(">-<");
        result.ShouldContain("first item");
    }

    [Fact]
    public void Ordered_list_marker_is_variable_class()
    {
        var result = _sut.HighlightToHtml("1. ordered");
        result.ShouldContain("class=\"md-li\"");
        result.ShouldContain(">1.<");
    }

    [Fact]
    public void Bold_marker_wraps_text_and_uses_em_class()
    {
        var result = _sut.HighlightToHtml("**bold**");
        result.ShouldContain("class=\"md-em\"");
        result.ShouldContain("**");
        result.ShouldContain("class=\"md-bold\"");
        result.ShouldContain(">bold<");
    }

    [Fact]
    public void Italic_underscore_marker_wraps_text_and_uses_em_class()
    {
        var result = _sut.HighlightToHtml("_italic_");
        result.ShouldContain("class=\"md-em\"");
        result.ShouldContain("class=\"md-italic\"");
        result.ShouldContain(">italic<");
    }

    [Fact]
    public void Inline_code_uses_code_class()
    {
        var result = _sut.HighlightToHtml("press `Ctrl+S` to save");
        result.ShouldContain("class=\"md-code\"");
        result.ShouldContain("Ctrl+S");
    }

    [Fact]
    public void Fenced_code_block_lines_use_codeblock_class()
    {
        var input = "```\nint x = 1;\nreturn x;\n```";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"md-codeblock\"");
        result.ShouldContain("int x = 1;");
        result.ShouldContain("return x;");
    }

    [Fact]
    public void Heading_inside_fenced_block_is_treated_as_code_not_heading()
    {
        var input = "```\n# Not a heading\n```";
        var result = _sut.HighlightToHtml(input);

        result.ShouldNotContain("class=\"md-hm\"");
        result.ShouldContain("class=\"md-codeblock\"");
    }

    [Fact]
    public void Link_text_and_url_are_separately_classed()
    {
        var result = _sut.HighlightToHtml("[click](https://example.com)");

        result.ShouldContain("class=\"md-link-text\"");
        result.ShouldContain(">click<");
        result.ShouldContain("class=\"md-link-url\"");
        result.ShouldContain("https://example.com");
        result.ShouldContain("class=\"md-link-pct\"");
    }

    [Fact]
    public void Blockquote_line_uses_bq_class()
    {
        var result = _sut.HighlightToHtml("> quoted");
        result.ShouldContain("class=\"md-bq\"");
        result.ShouldContain("quoted");
    }

    [Fact]
    public void Multiple_lines_are_joined_with_newlines()
    {
        var result = _sut.HighlightToHtml("# A\n# B");
        result.ShouldContain("\n");
    }

    [Fact]
    public void Round_trip_text_is_preserved_when_stripped_of_tags()
    {
        var input = "---\ntitle: Hi\n---\n\n# Hello\n\n**bold** and _italic_ and `code`.\n";
        var html = _sut.HighlightToHtml(input);

        // Strip span tags and decode HTML entities, the visible text should equal the input.
        var stripped = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
        var decoded = stripped.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
        decoded.ShouldBe(input);
    }

    [Fact]
    public void Yaml_list_item_with_key_value_marks_dash_as_punct_and_key_value_normally()
    {
        var input = "---\nfeatures:\n  - title: The Menagerie\n---\n";
        var result = _sut.HighlightToHtml(input);

        // The leading `- ` is a list marker (punctuation), then `title` is a key, value is a string.
        result.ShouldContain("class=\"yp\"");
        result.ShouldContain("class=\"yk\"");
        result.ShouldContain(">title<");
        result.ShouldContain("class=\"ys\"");
        result.ShouldContain(">The Menagerie<");
    }

    [Fact]
    public void Yaml_bare_list_item_marks_dash_as_punct_and_value_as_string()
    {
        var input = "---\ntags:\n  - csharp\n---\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yp\"");
        result.ShouldContain("class=\"ys\"");
        result.ShouldContain(">csharp<");
    }

    [Fact]
    public void Html_open_and_close_style_tags_use_html_class()
    {
        var input = "<style>\nbody { color: red; }\n</style>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"html\"");
        result.ShouldContain("&lt;style&gt;");
        result.ShouldContain("&lt;/style&gt;");
    }

    [Fact]
    public void Css_selector_and_braces_inside_style_block_are_classed()
    {
        var input = "<style>\n.foo {\n}\n</style>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"css-sel\"");
        result.ShouldContain(">.foo<");
        result.ShouldContain("class=\"css-punct\"");
    }

    [Fact]
    public void Css_property_and_value_inside_style_block_are_classed()
    {
        var input = "<style>\n.foo {\n  border-radius: 50%;\n}\n</style>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"css-prop\"");
        result.ShouldContain(">border-radius<");
        result.ShouldContain("class=\"css-val\"");
        result.ShouldContain("50%");
    }

    [Fact]
    public void Css_at_rule_is_classed()
    {
        var input = "<style>\n@media (max-width: 768px) {\n  .foo { color: red; }\n}\n</style>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"css-at\"");
        result.ShouldContain(">@media<");
    }

    [Fact]
    public void Css_outside_style_block_is_not_classed()
    {
        // A line that looks like CSS but isn't inside <style> stays plain markdown.
        var input = ".foo { color: red; }";
        var result = _sut.HighlightToHtml(input);

        result.ShouldNotContain("class=\"css-sel\"");
        result.ShouldNotContain("class=\"css-prop\"");
    }

    [Fact]
    public void Style_block_round_trips_when_stripped_of_tags()
    {
        var input = "<style>\nimg.VPImage.image-src {\n  border-radius: 50%;\n}\n\n@media (max-width: 768px) {\n  .foo { color: red; }\n}\n</style>\n";
        var html = _sut.HighlightToHtml(input);
        var stripped = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
        var decoded = stripped.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
        decoded.ShouldBe(input);
    }

    [Fact]
    public void Html_open_and_close_script_tags_use_html_class()
    {
        var input = "<script>\nconst x = 1;\n</script>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"html\"");
        result.ShouldContain("&lt;script&gt;");
        result.ShouldContain("&lt;/script&gt;");
    }

    [Fact]
    public void Js_keyword_inside_script_block_is_classed()
    {
        var input = "<script>\nconst x = 1;\n</script>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"js-kw\"");
        result.ShouldContain(">const<");
    }

    [Fact]
    public void Js_number_inside_script_block_is_classed()
    {
        var input = "<script>\nconst x = 42;\n</script>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"js-num\"");
        result.ShouldContain(">42<");
    }

    [Fact]
    public void Js_object_literal_key_is_classed()
    {
        // Identifier immediately followed by a colon is a property key -> orange (like yaml keys).
        var input = "<script>\nconst o = { name: 'x', avatar: '/a.png' };\n</script>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"js-key\"");
        result.ShouldContain(">name<");
        result.ShouldContain(">avatar<");
    }

    [Fact]
    public void Js_string_inside_script_block_is_classed()
    {
        var input = "<script>\nconst x = 'hello';\n</script>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"js-str\"");
        result.ShouldContain("hello");
    }

    [Fact]
    public void Js_line_comment_inside_script_block_is_classed()
    {
        var input = "<script>\n// a comment\n</script>\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"js-comment\"");
        result.ShouldContain("a comment");
    }

    [Fact]
    public void Js_outside_script_block_is_not_classed()
    {
        var input = "const x = 1;";
        var result = _sut.HighlightToHtml(input);

        result.ShouldNotContain("class=\"js-kw\"");
        result.ShouldNotContain("class=\"js-num\"");
    }

    [Fact]
    public void Script_block_round_trips_when_stripped_of_tags()
    {
        var input = "<script>\nconst x = 42;\n// a comment\nconst s = 'hi';\nfunction f() { return x; }\n</script>\n";
        var html = _sut.HighlightToHtml(input);
        var stripped = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
        var decoded = stripped.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
        decoded.ShouldBe(input);
    }

    [Fact]
    public void Void_html_tag_in_markdown_body_is_classed_as_html()
    {
        // Self-closing tags like <br/>, <hr/> show up unsupported by markdown.
        var result = _sut.HighlightToHtml("<br/>");

        result.ShouldContain("class=\"html\"");
        result.ShouldContain("&lt;br");
        result.ShouldContain("/&gt;");
    }

    [Fact]
    public void Vue_component_tag_with_attributes_is_classed()
    {
        // VitePress / Vue authors regularly drop `<Component :prop="val"/>` into Markdown.
        var result = _sut.HighlightToHtml("<VPTeamMembers :members=\"family\"/>");

        result.ShouldContain("class=\"html\"");
        result.ShouldContain("VPTeamMembers");
    }

    [Fact]
    public void Inline_html_tag_attribute_name_is_classed()
    {
        var result = _sut.HighlightToHtml("<a href=\"https://example.com\">link</a>");

        // Attribute name should get its own class so it can read differently from the tag name.
        result.ShouldContain("class=\"html-attr\"");
        result.ShouldContain(">href<");
    }

    [Fact]
    public void Inline_html_tag_attribute_value_is_classed_as_string()
    {
        var result = _sut.HighlightToHtml("<a href=\"https://example.com\">link</a>");

        result.ShouldContain("class=\"html-str\"");
        result.ShouldContain("https://example.com");
    }

    [Fact]
    public void Inline_html_close_tag_is_classed()
    {
        var result = _sut.HighlightToHtml("<div>x</div>");

        // Both the open and the close are classed; the inner text stays unstyled markdown.
        result.ShouldContain("&lt;div");
        result.ShouldContain("&lt;/div");
        var openCount = System.Text.RegularExpressions.Regex.Count(result, "class=\"html\"");
        openCount.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public void Inline_html_tag_round_trips_when_stripped_of_tags()
    {
        var input = "<VPTeamMembers :members=\"family\"/>\n<br/>\nplain text\n";
        var html = _sut.HighlightToHtml(input);
        var stripped = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
        var decoded = stripped.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
        decoded.ShouldBe(input);
    }

    [Fact]
    public void Inline_html_tag_inside_style_block_is_not_re_tokenized()
    {
        // Inside <style>, a stray `<` belongs to CSS land, not HTML-tag land.
        var input = "<style>\n.foo { color: red; }\n</style>\n";
        var result = _sut.HighlightToHtml(input);

        // The `.foo` selector is css-sel, not html-attr or html. (Spot check.)
        result.ShouldContain("class=\"css-sel\"");
    }

    [Fact]
    public void Crlf_line_endings_do_not_break_frontmatter_highlighting()
    {
        // Windows-authored files arrive with \r\n. Before line-ending normalization the trailing
        // \r defeated every per-line regex (the `$` anchor won't match before a \r), so the whole
        // frontmatter rendered as raw, unhighlighted (white) text.
        var input = "---\r\ntitle: Hello World\r\n---\r\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yk\"");
        result.ShouldContain(">title<");
        result.ShouldContain("class=\"ys\"");
        result.ShouldContain(">Hello World<");
        result.ShouldNotContain("\r");
    }

    [Fact]
    public void Leading_utf8_bom_does_not_break_frontmatter_detection()
    {
        // A leading BOM made line 0 != "---", so frontmatter was never detected and the block
        // fell back to plain markdown.
        var input = "\uFEFF---\ntitle: Hello\n---\n";
        var result = _sut.HighlightToHtml(input);

        result.ShouldContain("class=\"yk\"");
        result.ShouldContain(">title<");
        result.ShouldContain("class=\"ys\"");
        result.ShouldContain(">Hello<");
        result.ShouldNotContain("\uFEFF");
    }
}
