# Preview syntax highlighting — design

**Issue:** [#23 "Highlight Dot JS"](https://github.com/BlueFenixProductions/PostXING/issues/23)
**Date:** 2026-06-09
**Status:** Approved design, pending implementation plan.

## Problem

PostXING's preview pane renders the post body through Markdig into `.markdown-body` HTML, styled by the bundled GitHub colorblind CSS and the active theme palette (`PreviewRenderer.Build`). Markdig emits fenced code as `<pre><code class="language-x">…</code></pre>` with the **raw, uncolored** text — nothing generates syntax-token spans, so code blocks in the preview render monospace but plain. The blog this app publishes to is programming-focused, so unhighlighted code is a real readability gap.

The issue links highlight.js and the author noted a "verify squeezeworthiness" doubt, plus a tension with the project's colorblind-first accessibility stance.

## Decisions

1. **Palette constraint — standard theme is acceptable.** The preview is a convenience render; highlighting may use a stock highlight.js theme rather than a custom daltonized palette. (Operator decision — this is what removes the hard part of #23.)
2. **Approach — client-side highlight.js.** Bundle highlight.js offline and let it colorize in the WebView, rather than a server-side C# highlighter (e.g. ColorCode). Chosen because: it is literally what the issue asked for; best language support with auto-detect; least added C#; and the self-contained `<script>` runs entirely in the WebView with **no HybridWebView host bridge**, so the documented Android bridge limitations do not apply and behavior is identical on Windows and Android.

## Design

### Bundled assets (offline, no CDN)

Add to `Resources/Raw/preview/` (alongside the existing `github-markdown-*-colorblind.css`):

- `highlight.min.js` — the highlight.js **common** build (~35 languages: C#, JS/TS, bash, python, json, yaml, html, css, sql, …). Covers a programming blog at a fraction of the full ~190-language build; tens of KB, negligible for desktop/Android packaging.
- `highlight-github.css` — stock hljs light theme (matches the GitHub-README aesthetic already used).
- `highlight-github-dark.css` — stock hljs dark theme.

### `PreviewRenderer` (stays a pure function)

`PreviewRenderer.Build(...)` already accepts `githubCss` as a string and inlines it into `<style>`. Extend the signature to also accept `hljsJs` and `hljsThemeCss` strings, and in the built document:

- inline `<style>{hljsThemeCss}</style>` (after the existing github CSS and theme-override `<style>`),
- inline `<script>{hljsJs}</script>` and a bootstrap `<script>hljs.highlightAll()</script>` immediately before `</body>`.

No new I/O — the renderer remains pure; the caller supplies the asset strings exactly as it already supplies `githubCss`. The existing brightness/back-compat `Build` overload threads the same two new arguments through.

### Caller wiring

`IPreviewStyles` / `Services/PreviewStyles.cs` already loads `github-markdown-*-colorblind.css` from `Resources/Raw/preview/` (a `MauiAsset`) via `GithubMarkdownCss(bool dark)`. Extend that interface with two members — `HighlightJs()` (returns the bundled `highlight.min.js`) and `HighlightThemeCss(bool dark)` (returns `highlight-github-dark.css` for dark, `highlight-github.css` for light, keyed off the **same brightness signal** the github base CSS already uses). `PreviewViewModel.Build` (`src/PostXING.ViewModels/PreviewViewModel.cs`), which today calls `_renderer.Build(_markdown, _styles.GithubMarkdownCss(Dark), …)`, threads the two new strings into the extended `PreviewRenderer.Build`. Keeping the asset access behind `IPreviewStyles` preserves the renderer's purity and lets the unit tests supply a fake. One theme per brightness — **not** per-gallery-theme.

### Behavior

Markdig emits `<code class="language-csharp">`; `hljs.highlightAll()` reads the `language-*` class and colorizes on WebView load. Untagged fenced blocks fall back to hljs auto-detection. A brief post-load colorize flash is acceptable.

### Testing

`PreviewRenderer` is fully unit-tested today. Add tests asserting that `Build` output:
- contains the hljs bootstrap (`hljs.highlightAll`) and the supplied theme `<style>`,
- renders a fenced ` ```csharp ` block as `<code class="language-csharp">` so hljs can act on it.

The colorization itself is client-side JS and out of scope for a pure C# unit test — the tests verify the **wiring**, not pixel colors. This is stated explicitly so the coverage boundary is honest.

## Scope / non-goals (YAGNI)

- highlight.js **common** build only, not the full ~190-language bundle.
- One light + one dark theme; **no** per-gallery-theme highlight palettes.
- **No** custom daltonized highlight colors (per decision 1).
- No copy-button, line numbers, or language-label chrome — colorization only.

## Acceptance criteria

- A post with fenced code blocks renders with syntax colors in the preview on both Windows and Android.
- Highlighting works fully offline (assets bundled, no network).
- `PreviewRenderer` remains pure; new wiring tests pass; existing preview tests stay green.
- No regression to the frontmatter table, theme palette, or non-code body rendering.

## Open follow-ups (out of this spec)

- Whether to revisit a daltonized highlight palette later if the standard theme proves insufficient for colorblind users (the original #23 tension; deferred by decision 1).
