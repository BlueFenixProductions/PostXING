# Dictation discoverability — design

**Issue:** [#14 "Enable dictation"](https://github.com/BlueFenixProductions/PostXING/issues/14)
**Date:** 2026-06-10
**Status:** Approved design, pending implementation.

## Problem

Users want to dictate posts. Dictation **already works** via the OS — Android's keyboard mic (the editor's IME composition path in `index.html` is explicitly written to support it) and Windows `Win+H` dictate into any focused field, including the editor's contenteditable. What's missing is **discoverability**: there's no in-app cue, so users don't know they can dictate. The issue author leaned toward a docs how-to plus an `mdi-mic` cue rather than building a speech engine.

## Decisions (from the brainstorming interview)

1. **Deliver a docs how-to + a subtle in-app mic cue.** No speech-recognition engine. (Operator decision.)
2. **The cue focuses the editor and shows a transient, auto-dismissing hint** pointing at the OS dictation — no OS input injection. (Operator decision.)
3. **Use a real `mdi-microphone` icon** (a Material Design Icons font), not a text affordance — "the icon is visually useful to humans." (Operator decision; overrides the text-label house aesthetic for this one affordance.)
4. **Show the cue on both platforms** (not Windows-only). (Operator decision.)

## Design

### Material icon font

Register a Material Design Icons TTF in `Resources/Fonts/` (alongside Hack) via `MauiProgram.ConfigureFonts`, e.g. `fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialIcons")`. **Subset the font to the few glyphs used** (`microphone`, and `microphone-off` for a possible future toggle) to avoid shipping the full ~1.2 MB MDI webfont in the APK/MSIX — a subsetted TTF is a few KB. The mic glyph is referenced via a `FontImageSource` with `FontFamily="MaterialIcons"` and the `mdi-microphone` codepoint **U+F036C** -- above the BMP, so C# uses the surrogate pair `"\U000F036C"` and XAML uses the `&#xF036C;` entity. Some MDI builds remap glyphs into the BMP private-use range; if the bundled font does, use that codepoint instead.

### Cue placement

- **Windows** — a mic `Button` in the editor's bottom status bar (`EditorPage.xaml`, `Grid.Row=1`), using `ImageSource` = the `FontImageSource` mic glyph instead of `Text`. Sits with `new`/`open`/`settings`/…; the icon is the one non-text affordance, intentionally.
- **Android** — a **primary** `ToolbarItem` (visible in the Material top app bar, not the overflow) with `IconImageSource` = the same `FontImageSource`. The existing actions stay in the overflow (`Order="Secondary"`); the mic is promoted so it's a visible, tappable icon.

### Behavior

Tapping the mic:
1. **Focuses the editor** — `EditorWebView.EvaluateJavaScriptAsync("window.PostXING.focus()")` -- `window.PostXING.focus()` already exists in `index.html` (calls `editor.focus()`), so no JS change is needed. (Fire-and-forget on Android per the existing bridge pattern.)
2. **Shows a transient, auto-dismissing hint** with platform-specific text:
   - Windows: `Press Win+H to dictate`
   - Android: `Tap the mic on your keyboard to dictate`

The hint is a self-contained element (a `Border`+`Label` overlaid near the status bar) that fades in, waits ~2.5 s, and fades out — **no new dependency** (CommunityToolkit.Maui is not referenced). A single `DictationHintRequested` event / bool drives its visibility.

### Hint text helper (the testable seam)

The platform→hint-text mapping lives in a tiny pure helper (e.g. `DictationHints.For(DevicePlatform)` returning the string), so the one piece of logic is unit-testable off the MAUI TFM in `PostXING.ViewModels`. The focus call and the fade are view-layer (`EditorPage` code-behind), not unit-tested.

### Docs

`docs/dictation.md` — "Dictating posts in PostXING":
- **Windows:** press `Win+H` to open the dictation toolbar; speak; it types into the editor. Punctuation commands ("period", "new line"). The in-app mic button is a reminder/focus shortcut, not a separate engine.
- **Android:** tap the mic on your soft keyboard; the editor handles voice input (it's written to not drop composition mid-highlight).
- Note that this uses the OS's own speech recognition — nothing leaves the device beyond what the OS dictation already does.

## Acceptance criteria

- A mic icon (real `mdi-microphone` glyph) is visible in the editor on both Windows and Android.
- Tapping it focuses the editor and briefly shows the correct platform hint, then auto-dismisses.
- `docs/dictation.md` documents Win+H (Windows) and the keyboard mic (Android).
- The Material icon font is subsetted (not the full ~1.2 MB webfont).
- No speech-recognition code, no `Win+H` injection; existing editor behavior unchanged.

## Non-goals (YAGNI)

- No in-app speech-to-text engine (`System.Speech` / `Windows.Media.SpeechRecognition`).
- No programmatic `Win+H` injection.
- No mic-state/recording UI — the OS owns the dictation session.
