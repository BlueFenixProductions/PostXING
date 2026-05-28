---
name: PostXING 4.0
description: A Windows-only Markdown editor that wears the Blue Fenix design voice.
colors:
  phoenix-blue: "#1E5BFF"
  phoenix-blue-hover: "#4D7DFF"
  phoenix-blue-deep: "#0A2BC2"
  signal-cyan: "#06B6D4"
  forge-amber: "#E08A2E"
  forge-amber-hover: "#D97706"
  ember: "#7F170E"
  ink-1000: "#08081A"
  ink-950: "#0F0F1F"
  ink-850: "#1A1A2E"
  ink-700: "#2D2D4A"
  paper-100: "#FFFFFF"
  paper-300: "#E5E7EB"
  paper-500: "#9095A4"
  paper-600: "#6B7080"
  success: "#10B981"
  warning: "#F59E0B"
  danger: "#EF4444"
typography:
  display:
    fontFamily: "Space Grotesk, Inter, system-ui, sans-serif"
    fontSize: "32px"
    fontWeight: 700
    lineHeight: 1.15
    letterSpacing: "-0.02em"
  headline:
    fontFamily: "Space Grotesk, Inter, system-ui, sans-serif"
    fontSize: "24px"
    fontWeight: 700
    lineHeight: 1.2
    letterSpacing: "-0.015em"
  title:
    fontFamily: "Space Grotesk, Inter, system-ui, sans-serif"
    fontSize: "18px"
    fontWeight: 700
    lineHeight: 1.25
    letterSpacing: "-0.01em"
  body:
    fontFamily: "Hack, JetBrains Mono, ui-monospace, Consolas, monospace"
    fontSize: "14px"
    fontWeight: 400
    lineHeight: 1.55
    letterSpacing: "0"
  list:
    fontFamily: "Hack, JetBrains Mono, ui-monospace, monospace"
    fontSize: "13px"
    fontWeight: 400
    lineHeight: 1.5
  terminal:
    fontFamily: "Hack, JetBrains Mono, ui-monospace, monospace"
    fontSize: "13px"
    fontWeight: 400
    lineHeight: 1.6
  label:
    fontFamily: "Hack, JetBrains Mono, ui-monospace, monospace"
    fontSize: "11px"
    fontWeight: 500
    lineHeight: 1.3
    letterSpacing: "0.14em"
  kicker:
    fontFamily: "Hack, JetBrains Mono, ui-monospace, monospace"
    fontSize: "12px"
    fontWeight: 500
    lineHeight: 1.3
    letterSpacing: "0.14em"
rounded:
  sharp: "0px"
  sm: "4px"
  md: "8px"
  lg: "12px"
spacing:
  '1': "4px"
  '2': "8px"
  '3': "12px"
  '4': "16px"
  '5': "20px"
  '6': "24px"
  '8': "32px"
  '12': "48px"
  '16': "64px"
components:
  editor-surface-dark:
    backgroundColor: "{colors.ink-950}"
    textColor: "{colors.paper-300}"
    typography: "{typography.body}"
    padding: "2px"
  editor-surface-light:
    backgroundColor: "{colors.paper-100}"
    textColor: "{colors.ink-950}"
    typography: "{typography.body}"
    padding: "2px"
  status-tap-label:
    backgroundColor: "transparent"
    textColor: "{colors.paper-500}"
    typography: "{typography.label}"
    rounded: "{rounded.sharp}"
    padding: "0"
  status-tap-label-primary:
    backgroundColor: "transparent"
    textColor: "{colors.phoenix-blue}"
  status-tap-label-cyan:
    backgroundColor: "transparent"
    textColor: "{colors.signal-cyan}"
  kicker:
    backgroundColor: "transparent"
    textColor: "{colors.signal-cyan}"
    typography: "{typography.kicker}"
    padding: "0"
  title-prompt-overlay:
    backgroundColor: "{colors.ink-850}"
    textColor: "{colors.paper-100}"
    rounded: "{rounded.md}"
    padding: "56px"
  title-prompt-heading:
    backgroundColor: "transparent"
    textColor: "{colors.paper-100}"
    typography: "{typography.display}"
  title-prompt-entry:
    backgroundColor: "transparent"
    textColor: "{colors.paper-100}"
    typography: "{typography.headline}"
    padding: "0"
  list-row:
    backgroundColor: "transparent"
    textColor: "{colors.paper-300}"
    typography: "{typography.list}"
    padding: "8px 4px"
  list-row-tag:
    backgroundColor: "transparent"
    textColor: "{colors.paper-500}"
    typography: "{typography.label}"
  button-primary:
    backgroundColor: "{colors.phoenix-blue}"
    textColor: "{colors.paper-100}"
    rounded: "{rounded.sm}"
    padding: "0 22px"
    height: "44px"
    typography: "{typography.label}"
  button-primary-hover:
    backgroundColor: "{colors.phoenix-blue-hover}"
    textColor: "{colors.paper-100}"
  button-ghost:
    backgroundColor: "transparent"
    textColor: "{colors.paper-300}"
    rounded: "{rounded.sm}"
    padding: "0 22px"
    height: "44px"
    typography: "{typography.label}"
  divider:
    backgroundColor: "{colors.ink-700}"
    height: "1px"
  terminal-canvas:
    backgroundColor: "{colors.ink-1000}"
    textColor: "{colors.paper-300}"
    typography: "{typography.terminal}"
    rounded: "{rounded.lg}"
    padding: "20px 24px"
  terminal-prompt:
    backgroundColor: "transparent"
    textColor: "{colors.signal-cyan}"
    typography: "{typography.terminal}"
  terminal-output:
    backgroundColor: "transparent"
    textColor: "{colors.paper-300}"
    typography: "{typography.terminal}"
  terminal-accent:
    backgroundColor: "transparent"
    textColor: "{colors.forge-amber}"
    typography: "{typography.terminal}"
  terminal-ok-token:
    backgroundColor: "transparent"
    textColor: "{colors.success}"
    typography: "{typography.terminal}"
---

# Design System: PostXING 4.0

## 1. Overview

**Creative North Star: "The Workbench, Wired to the Forge"**

PostXING is the writing surface of Blue Fenix Productions. It wears the studio's design DNA without translation: ink-950 (`#0F0F1F`) is the canvas, Hack monospace is the body face, phoenix blue (`#1E5BFF`) does the interactive work, signal cyan (`#06B6D4`) marks the live signals, forge amber (`#E08A2E`) is the editorial heat-on-cold-metal, ember (`#7F170E`) is reserved for historical accents only. Below 24px every glyph is Hack; at 24px and above the display voice switches to Space Grotesk. Section labels arrive in `[ bracketed lowercase ]` cyan mono, the same kicker idiom the marketing site uses. The gh terminal page is the studio's terminal block, not a generic green-on-black CRT pastiche.

The personality is the brand's: *tempered, direct, slightly wry. Professional geek, shipping since 2001.* The tool is dense without being noisy. There is no title bar, no toolbar, no sidebar, no ribbon, no breadcrumb. The thin status bar at the bottom of the editor is the only persistent chrome, and the commands there read as quiet caps-mono kickers rather than buttons. The writer is meant to forget the tool exists within two seconds of launching it, and to recognize the studio's hand whenever they look up.

A product-register concession the marketing site does not have to make: PostXING supports a light "writer's daylight" mode alongside the dark default. The brand's *"dark canvas only"* rule governs marketing surfaces; in a tool a writer spends hours inside, light mode for daytime work is a legitimate accommodation. Both modes use the Blue Fenix token set with their relationships inverted, so the brand identity survives the switch.

This system rejects every anti-reference the marketing site already names: generic SaaS landing-page surfaces (cream hero, floating feature cards, gradient CTAs); corporate-consulting hierarchy (navy plus gold, serif wordmarks, stock photography); agency-portfolio maximalism (oversized type that fights itself, cursor effects, scroll choreography). It also rejects the product-register variants the marketing site does not need to worry about: Notion-style slash menus and hover toolbars, identical icon-plus-heading-plus-body card walls, side-stripe colored borders, hero-metric templates, glassmorphism, gradient text, and em dashes in any copy.

**Key Characteristics:**

- Ink-950 is the dark-mode canvas; paper-100 is the light-mode canvas. The same token system, inverted.
- Hack monospace is the body face. Hack carries every paragraph, label, status read, list row, and form input.
- Space Grotesk is the display voice. It appears only at 24px and above.
- Phoenix blue does the interactive work. Signal cyan marks live signals. Forge amber is editorial heat. Ember is reserved historical.
- Bracketed kicker labels (`[ local posts folder ]`, `[ github cli ]`) are the brand's signature, repeated in the editor.
- The gh terminal page is the studio's terminal block, not a green CRT.
- Flat by default; the one explicit corner radius outside buttons (8px) is on the title-prompt overlay, deliberately, to signal a modal moment.
- Light mode exists as the writer's-daylight exception. The token set survives the inversion.

## 2. Colors

A dark surface stack tinted toward the brand hue, two interactive accents (phoenix blue, signal cyan), one warm anchor (forge amber), and a single historical color (ember) carried straight from the studio palette.

### Primary

- **Phoenix Blue** (`#1E5BFF`): The namesake doing real work. Primary CTAs (`Start writing`, `Save` in the editor, `Save` in Settings), the brand's identity color, focus rings on text entries, the current-state highlight in the status bar when a command is the focus of attention. Hover lifts to `#4D7DFF`. Pressed deepens to `#0A2BC2`.

### Secondary

- **Forge Amber** (`#E08A2E`): Editorial warmth. The `[exit 0]` token in the gh terminal output, the `gh ok` and `gh: <user>` status read at the right edge of the editor status bar, the underline under a quote inside rendered Markdown preview. Reads as warmth on cold metal: where the eye should land for editorial moments. Hover deepens to `#D97706`. Never used as a button background, never as a body color, never as an alert.

### Tertiary

- **Signal Cyan** (`#06B6D4`): The live signal. Bracketed kickers across Settings sections (`[ local posts folder ]`, `[ github repository ]`, `[ author ]`, `[ github cli ]`), the `~/blue-fenix $` prompt in the gh terminal, the focus ring on text entries, the "{n} words" / "{n} min" reads in the editor status bar. Cyan is the pulse, not a CTA color.

### Neutral (Ink)

- **Ink-1000** (`#08081A`): Sunken surfaces. The gh terminal canvas. Hero-area shadow when one is needed.
- **Ink-950** (`#0F0F1F`): The page canvas in dark mode. Every editor surface, every page background, sits on this unless explicitly raised.
- **Ink-850** (`#1A1A2E`): Elevated surfaces. The title-prompt overlay panel, the Settings stack background when a section needs to feel raised.
- **Ink-700** (`#2D2D4A`): Raised surfaces. Hovered list rows, the Settings page's section dividers, drawer-style affordances if ever introduced.

### Neutral (Paper)

- **Paper-100** (`#FFFFFF`): Strong foreground only in dark mode (display headlines, the title-prompt entry text, primary button labels). In light mode, the page canvas.
- **Paper-300** (`#E5E7EB`): Default foreground in dark mode. Body copy, list rows, form values, terminal output.
- **Paper-500** (`#9095A4`): Muted foreground. Section ledes, status-bar reads, list-row tags (`local`, `github draft`, `github post`), placeholder text on form fields.
- **Paper-600** (`#6B7080`): Subtle foreground. Helper lines under settings fields, caption-level secondary text.

### Reserved

- **Ember** (`#7F170E`): Historical accent only. Reserved for any future "this is the legacy of PostXING v2 from 2007" reference inside the app. A reminder that the phoenix burned. **Never use for errors**; the danger token (`#EF4444`) handles those.

### Status

- **Success** (`#10B981`) for the `ok` token in terminal output and for a successful `Saved {file}` status read. **Warning** (`#F59E0B`) for transient cautions. **Danger** (`#EF4444`) for failed-save and failed-auth states. Status is utility, not identity; do not promote these to brand colors.

### Named Rules

**The Five-Color Rule (carried from the studio).** Roughly 85% of any screen is ink + paper neutrals. Phoenix blue does interactive work. Cyan does live-signal work. Amber does editorial work. Ember is reserved historical. If a fifth hue is being introduced, the answer is almost always to use the existing tokens differently.

**The Phoenix-Versus-Cyan Rule.** Phoenix blue is for buttons and identity moments. Cyan is for kickers, prompts, focus rings. Do not collapse them into one accent.

**The Heat-on-Cold-Metal Rule.** Forge amber appears where the eye should warm up. In PostXING that is the `[exit 0]` token, the `gh ok` status read, an editorial accent inside rendered Markdown preview. Never as a body color, never as a button background, never as an alert.

**The Ember-Is-Historical Rule.** Ember (`#7F170E`) is reserved for any future Blue-Fenix-historical moment ("a reminder that the phoenix burned"). It is not the error color, not the dirty-file marker, not the placeholder accent. If the editor never grows a historical reference, ember never ships.

## 3. Typography

**Display Font:** Space Grotesk (with Inter, system-ui as fallback).
**Body Font:** Hack (with JetBrains Mono, ui-monospace, Consolas as fallback), MIT/OFL, bundled at `Resources/Fonts/Hack-Regular.ttf` and `Hack-Bold.ttf` and registered in `MauiProgram.ConfigureFonts` as `HackRegular` and `HackBold`.

**Character:** A monospace body face is the studio's signature, carried into the editor unchanged. Hack carries every paragraph, every label, every status read. Space Grotesk steps in only when monospace runs out of weight at display sizes (≥24px). PostXING does not use a wordmark face today (Century Gothic stays reserved for any future brand lockup inside the app).

### Hierarchy

- **Display** (Space Grotesk, 700, 32px, line-height 1.15, tracking -0.02em): The title-prompt overlay heading ("What are we writing about?"). Hero moments only.
- **Headline** (Space Grotesk, 700, 24px, line-height 1.2): The title-prompt entry's typed title at 28px effective on a 72px row. The Settings page heading.
- **Title** (Space Grotesk, 700, 18px, line-height 1.25): The Open page heading. Any future "section title" treatment heavier than a kicker.
- **Body** (Hack, 400, 14px, line-height 1.55): The editor body. 2px outer margin so the text never kisses the window edge. The single most important type role.
- **List** (Hack, 400, 13px, line-height 1.5): List-row display names on the Open page.
- **Terminal** (Hack, 400, 13px, line-height 1.6): The gh terminal output, command input, and prompt body.
- **Label** (Hack, 500, 11px, line-height 1.3, tracking 0.14em, uppercase): Every status-bar tap label, every form-field caption on Settings, list-row tags. The brand-mono caps treatment.
- **Kicker** (Hack, 500, 12px, line-height 1.3, tracking 0.14em, uppercase, bracketed): Section labels on Settings (`[ local posts folder ]`, `[ github repository ]`, `[ author ]`, `[ github cli ]`). Once per major section, sitting above the field. The brackets are rendered at 50% opacity to match the studio's `::before`/`::after` treatment.

### Named Rules

**The Mono-Body Rule.** Hack is the body face and is not negotiable. If long-form reading suggests a switch, the answer is to widen the leading or tighten the line length, not to swap the family.

**The 24px Switch Rule.** Below 24px, type stays in Hack. At 24px and above, switch to Space Grotesk. Never mix faces inside a single line.

**The Bracketed-Kicker Rule.** Section labels are wrapped in `[ ]` brackets and lowercase mono caps. The brackets render at 50% opacity; the label itself sits in signal cyan. Without the brackets the kicker is just an eyebrow, and any product has those.

**The 14pt Body Rule.** The editor body is Hack-Regular at 14pt with a 2px margin from the window edge. This combination is load-bearing; it is the answer to *what does the page look like*. Changing it requires a reason as strong as the one that put it there.

## 4. Elevation

Flat by default. Depth is conveyed through the ink stack (`ink-1000` sunken, `ink-950` page, `ink-850` elevated, `ink-700` raised) and through shadow only for two roles: containing the title-prompt overlay, and adding selective glow under accent elements.

### Shadow Vocabulary

- **Shadow-2** (`0 4px 12px rgba(0,0,0,0.40)`): The terminal block, when it sits on the ink-950 page and needs to read as resting on the surface.
- **Shadow-3** (`0 8px 24px rgba(0,0,0,0.50)`): The title-prompt overlay panel. The only modal moment.
- **Glow-Cyan** (`0 0 0 1px rgba(6,182,212,0.40), 0 0 24px rgba(6,182,212,0.25)`): Focus ring on text entries (Settings fields, the title-prompt title entry, the gh terminal command input).
- **Glow-Phoenix** (`0 0 24px rgba(30,91,255,0.35)`): Hover state on the primary button (`Start writing`, `Save`). Once per active surface.

### Named Rules

**The Flat-By-Default Rule.** Surfaces are flat at rest. A shadow appears only when an element is actively hovered, focused, or lifted out of the document flow. If you reach for a resting drop shadow, raise the surface tier instead.

**The Glow-Is-Rare Rule.** The cyan and phoenix glows appear at most once per visible surface. Two glowing elements in the same viewport means one is decoration.

**The One-Overlay Rule.** The title-prompt overlay is the only sanctioned modal moment in the editor. It earns its 8px radius and its `rgba(0,0,0,0.55)` scrim because it is interrupting the writer. Anything else that wants to be a modal should be inline or progressive instead.

## 5. Components

### Editor Surface
- **Character:** The entire page below the status bar is a single `Editor`. No internal padding beyond a 2px outer margin.
- **Typography:** Hack-Regular at 14pt, theme-bound ink color (`paper-300` on dark, `ink-950` on light).
- **Placeholder:** the verbatim string `"Start writing..."` in `paper-500`.
- **AutoSize:** `TextChanges` so the editor grows with the post.

### Status Bar (Editor)
- **Character:** A thin row at the bottom of the editor, padded 6px horizontal / 4px vertical. Reads as a sentence, not a toolbar.
- **Left cluster:** Tap labels for `New`, `Open`, `Settings`, `Save`, `Publish`. Each is 11pt Hack caps-mono at 0.14em tracking. Default color is `paper-500`. The currently-actionable command lifts to `phoenix-blue`. `Save` and `Publish` are hidden until `IsDirty`.
- **Right cluster:** Status reads in 11pt Hack: save status (`paper-500` default, `success` when freshly saved, `danger` when failed), current path (`paper-500`), word count and reading time (`signal-cyan` numerics inside `paper-500` units, e.g. `247 words` / `2 min`), gh auth status (`forge-amber` when `gh ok`, `paper-500` when `gh ?`, `danger` when `gh: not auth`).
- **States:** Today there is no hover or focus state on these tap labels. This is a known accessibility shortfall and should be addressed the next time the status bar is touched (see Do's and Don'ts).

### Bracketed Kicker
- **Character:** The brand's signature label. Used on Settings to mark section starts. 12pt Hack caps-mono at 0.14em tracking, signal-cyan, with `[ ` and ` ]` at 50% opacity on either side of the lowercase label.
- **Examples in Settings:** `[ local posts folder ]`, `[ github repository ]`, `[ integration branch ]`, `[ author ]`, `[ github cli ]`.
- **Frequency:** once per major section, sitting above the first field of that section. Replaces the current 11pt `paper-500` field labels at the section level. Per-field captions stay as 11pt `paper-500` label-mono below each `Entry`.

### Title-prompt Overlay
- **Character:** The one modal moment. Dims the editor with an `rgba(0,0,0,0.55)` scrim (lighter in light mode) and floats an 8px-cornered `ink-850` panel.
- **Layout:** 64px outer margin, 56px inner padding, 32px row spacing between heading, body, and action row.
- **Heading:** 32pt Space Grotesk Display ("What are we writing about?"), `paper-100`.
- **Entries:** Title at 28pt Headline-tier on a 72px row, `paper-100`. Author at 18pt Title-tier below, `paper-300`. Both transparent backgrounds.
- **Actions:** "Cancel" as a ghost button (`button-ghost`), "Start writing" as a primary button (`button-primary`).

### List Row (Open Post)
- **Character:** A flat row, padded 8px horizontal / 4px vertical. Display name in 13pt Hack `paper-300`; tag in 11pt label-mono caps `paper-500` (`local`, `github draft`, `github post`).
- **Hover:** background lifts to `ink-850` at 40% opacity. The display name shifts to `paper-100`.
- **No row dividers, no zebra striping.** The Settings divider style (1px in `ink-700`) is reserved for section breaks.

### Settings Stack
- **Character:** A single centered `VerticalStackLayout` at 720px `MaximumWidthRequest`, padded 24px, with 16px row spacing.
- **Section pattern:** a bracketed kicker for the section, the relevant `Entry` (or grid of entries) at the brand input style, then optionally an 11pt `paper-500` helper line beneath.
- **Section dividers:** a 1px `BoxView` in `ink-700` with 16px vertical margin. The only structural rule outside the editor.
- **Actions:** "Cancel" as `button-ghost`, "Save" as `button-primary` at the bottom right.

### Buttons (general)
- **Primary** (`button-primary`): Phoenix-blue background, paper-100 label, 11pt caps-mono at 0.14em tracking, 4px corner radius, 44px height, 0/22px padding. Hover lifts to `phoenix-blue-hover` with `glow-phoenix`. Used for: "Start writing", "Save" (in the title prompt and Settings).
- **Ghost** (`button-ghost`): Transparent background, paper-300 label, 1px border at `rgba(255,255,255,0.16)` (dark) or `rgba(0,0,0,0.12)` (light), 4px corner radius, 44px height. Hover lifts the border opacity. Used for: "Cancel", "Re-check status", "Open gh terminal".
- **Focus:** any keyboard focus on a button or text entry shows the 2px signal-cyan ring at 2px offset.

### Inputs / Text Entries
- **Character:** `ink-850` background in dark mode (`paper-100` with a 1px ink-700 border in light mode), `paper-300` text, 4px corner radius, ~48px height (`density="comfortable"` equivalent).
- **Focus:** outline shifts to phoenix-blue at 2px. Global focus-visible cyan ring takes precedence on keyboard focus.
- **Placeholder:** `paper-500` in both modes.

### Terminal Block (gh terminal page)
- **Character:** the studio's terminal block, made interactive. `ink-1000` sunken canvas with a 12px corner radius, 1px border in `rgba(255,255,255,0.06)`, `shadow-2`.
- **Page chrome:** the page title sits in 14pt Hack-Bold with the `[ gh terminal ]` kicker treatment. `Clear` and `Close` are `button-ghost` at the top right.
- **Output area:** a `ScrollView` inside the terminal canvas. Body in 13pt Hack `paper-300`. Prompt echoes (`$ gh <command>`) in signal-cyan. `ok` tokens in success-green. `[exit 0]` in forge-amber when the exit code is zero, danger-red when non-zero.
- **Prompt + input:** the prompt `~/blue-fenix $ gh` in signal-cyan caps-mono (lowercase but caps-mono via the kicker treatment), the borderless `Entry` in `paper-300` 13pt Hack on `ink-1000`, the `Run` button as `button-primary`.
- **Optional stdin:** a 60px-min `Editor` below the prompt, used for `auth login --with-token`. Same color/typography as the command input.
- **Docs footer:** `[ docs ]` kicker, then `https://cli.github.com/manual/` as a forge-amber anchor with a 1px underline.

### Dividers
- **Settings divider:** 1px `BoxView` in `ink-700`, 16px vertical margin between sections.
- **Editor / status-bar separation:** not a rule. The 11pt Ash type at the bottom of the page is the entire visual cue.

## 6. Do's and Don'ts

### Do:
- **Do** carry the body face in Hack. Mono is the studio voice; the editor inherits it.
- **Do** keep the dark-mode canvas at `ink-950` (`#0F0F1F`) and the light-mode canvas at `paper-100` (`#FFFFFF`). When a surface needs contrast, raise the tier (`ink-850`, `ink-700`) or sink it (`ink-1000`); do not introduce a new neutral.
- **Do** wrap section labels in bracketed lowercase mono caps via the kicker style. The brackets are part of the voice.
- **Do** write display headlines in four to eight words, sentence case. The title-prompt heading ("What are we writing about?") is exactly this.
- **Do** restrict forge amber to editorial moments: the `[exit 0]` token, the `gh ok` status read, an underline beneath a quote in rendered preview.
- **Do** route every keyboard-focus state through the 2px signal-cyan ring at 2px offset.
- **Do** respect `prefers-reduced-motion`. There is essentially no motion in the app today; any new motion must observe the preference.
- **Do** hide commands that don't apply. `Save` and `Publish` stay hidden until `IsDirty`. The status bar's width is part of its meaning.
- **Do** convey every status with text first; let color be atmosphere.

### Don't:
- **Don't** design PostXING on a paper-white surface as the default. That is the **generic SaaS landing page** anti-reference (cream/white hero, floating feature cards, gradient CTAs). Light mode is the writer's-daylight exception, not the default.
- **Don't** introduce serif wordmarks, navy + gold palettes, or stock photography. That is the **corporate consulting / Big Four** anti-reference.
- **Don't** add cursor effects, scroll choreography, or oversized type that fights itself. That is the **agency portfolio maximalism** anti-reference.
- **Don't** introduce slash menus, hover toolbars, plus-buttons on blank lines, drag handles, block models, database-as-blog, or any other piece of **Notion / Coda** vocabulary. The cursor in a flat text area is the only affordance.
- **Don't** use `border-left` or `border-right` greater than 1px as a colored stripe on cards, list rows, callouts, blockquotes, or alerts. Side-stripe accents are forbidden.
- **Don't** apply `background-clip: text` with a gradient. Gradient text is decorative, never meaningful.
- **Don't** decorate with glassmorphism. Backdrop blur is allowed only as a functional aid on a sticky chrome surface; PostXING has no such surface today, so the answer here is "not at all".
- **Don't** build a hero-metric block anywhere. The big-number / small-label / supporting-stats template is forbidden.
- **Don't** repeat identical icon-plus-heading-plus-body cards endlessly. The Open page is a flat list and stays one.
- **Don't** reach for a modal as the first thought. The title-prompt overlay is the only sanctioned modal; if a new flow seems to need one, exhaust inline or progressive alternatives first.
- **Don't** use em dashes in copy. Use commas, colons, semicolons, periods, or parentheses. For ranges, use an en dash (`–`) or hyphen (`-`).
- **Don't** ship `#000` or `#FFF` raw. Use the ink and paper tokens. `paper-100` is `#FFFFFF` only because the brand explicitly chose it as a token; `ink-950` (`#0F0F1F`) is the dark canvas, never `#0F0F0F` (the current code is wrong by 17 in the blue channel and the audit will surface it).
- **Don't** introduce a second design system on any future preview pane or rendered-markdown view. The same tokens, the same kicker, the same fonts apply everywhere.
- **Don't** use ember red for errors. Ember is reserved historical. The `danger` token handles errors.
- **Don't** soften the gh terminal page toward the rest of the app's neutrality. The terminal block is the studio's signature; it stays in the Blue Fenix terminal palette (ink-1000 / cyan prompt / amber accents / success ok) and not the legacy pure-green-on-black palette currently in the code.
- **Don't** auto-fill a new post with template frontmatter before the writer has committed a title through the prompt overlay. The blank-canvas cure depends on it.
- **Don't** introduce a `DraftsFolder` / `PostsFolder` setting, year/month subdirectories, or any other relaxation of the flat folder convention.
