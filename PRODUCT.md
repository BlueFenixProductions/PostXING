# Product

## Register

product

## Users

A single user: Chris Pelatari. 25 years of C#, ASP MVP era ~2004–2007, author of the original PostXING (2007) and founder of Blue Fenix Productions LLC. Writes long-form Markdown posts and publishes them through a Markdown + GitHub + static-site-generator workflow. Lives on Windows, edits in his own tool, commits with the git client of his choice.

No second user is planned. There is no download story, no public release, no telemetry, no onboarding for newcomers. Design decisions optimize for his muscle memory and personal taste, not for accessibility breadth or category familiarity.

## Product Purpose

A Windows-only Markdown editor for one writer's blogging workflow. The default flow is: open the local posts folder, write in Hack monospace on a Blue-Fenix-themed surface, save to disk, commit and push with whatever git client is convenient. The `gh` CLI integration and Publish flow are present as power-user shortcuts; the app is fully useful without `gh` installed.

Success looks like: launching the app and writing within two seconds; never seeing chrome that does not serve the prose; never being asked to learn a slash menu, a block model, or a hover toolbar; and trusting that the file on disk is the source of truth. PostXING is a Blue Fenix production, and it reads that way: tempered, direct, slightly wry.

## Brand Personality

The voice from bluefenixproductions.com, applied to a tool. **Tempered. Direct. Slightly wry.** Plainspoken, technical, occasionally dry. *"Professional geek, shipping since 2001."* *"We're not here to move fast and break things. We've seen what that leaves behind."* Never hyped, never apologetic.

The studio's design DNA carries into the editor unchanged: **dark canvas as the default**, **Hack monospace as the body face**, **bracketed-kicker section labels** in signal cyan, **phoenix-blue** doing the interactive work, **forge amber** as the warm editorial anchor, **a terminal block as a brand signature** rather than an afterthought. PostXING is not an unrelated tool that happens to be written by the same person; it is the studio's writing surface and should look like a sibling of the marketing site.

A product-register concession: the editor supports a light "writer's daylight" mode in addition to the dark default. The brand's *"dark canvas only"* rule governs marketing surfaces where the page itself is the product; in a tool a writer spends hours inside, light mode for daytime use is a legitimate accommodation. Both modes use the same Blue Fenix tokens with their relationships inverted.

## Anti-references

The same three anti-references the marketing site declares, applied to the editor:

- **Generic SaaS landing-page aesthetics carried into product UI.** Cream/white hero surfaces, floating feature cards, gradient CTAs, urgency copy, hero-metric templates, browser-mockup screenshots. None of this belongs anywhere in PostXING.
- **Corporate-consulting / Big-Four hierarchy.** Navy plus gold, serif wordmarks, stock photography, enterprise-formal section dividers. Forbidden.
- **Agency-portfolio maximalism.** Oversized type that fights itself, cursor effects, scroll choreography, design that performs effort. Forbidden.

Plus product-specific anti-references the marketing site does not need to worry about:

- **Notion / Coda vocabulary.** No slash menus, no hover toolbars, no plus-buttons on blank lines, no drag handles, no block models, no database-as-blog, no rounded-cards-on-rounded-cards SaaS chrome. The cursor in a flat text area is the only affordance.
- **Side-stripe colored borders** on cards, list rows, callouts, blockquotes, or alerts. Always wrong. (Restated from the marketing site's Don'ts because tools rediscover it constantly.)
- **Glassmorphism, gradient text, decorative blur** anywhere outside of a functional backdrop blur on a sticky chrome surface.
- **Em dashes in copy.** Use commas, colons, semicolons, periods, or parentheses. For ranges, an en dash or hyphen.

## Design Principles

1. **No theatre.** Animations are bounded micro-interactions or absent. Nothing makes the writer wait to read or type. If a motion choice is for the designer's amusement, cut it.
2. **Show, don't claim.** The work demonstrates the credentials. The editor doesn't need a "polished tool" banner; layout, precision, and restraint do that.
3. **Developer-native is the look.** Monospace body face, deep ink canvas, token-driven spacing, terminal block as a first-class component. This is the studio's character carried into a tool, not a stylistic flourish. Do not soften toward SaaS to look approachable.
4. **The tool disappears, the prose is the figure.** The editor body is the entire window minus a thin status bar at the bottom and a single MenuBar at the top. Toolbars, sidebars, ribbons, breadcrumbs, tab strips, drag handles, plus-buttons-on-blank-lines: all forbidden. The MenuBar exists because MAUI's `KeyboardAccelerator` only attaches to `MenuFlyoutItem` inside `ContentPage.MenuBarItems`, so the canonical Windows shortcuts (Ctrl+S, Ctrl+N, Ctrl+O, Ctrl+,, Ctrl+Shift+P) have nowhere else to live. Keep it to one top-level menu when possible, two at the outside; the menu carries shortcuts and standard desktop affordances, not decoration. If a new feature needs more chrome than the status bar plus a menu item to be discoverable, the feature is wrong, not the chrome budget.
5. **YAGNI is enforced.** No speculative abstractions, no cross-platform conditionals, no "future maybe" hooks, no alternative backends. Left-in dead code is worse than no code.
6. **Local files are first-class; `gh` is optional.** The app remains useful with `gh` uninstalled. The local-folder path is the default; the GitHub path is the power-user lane.
7. **The blank-canvas cure.** New posts start with an empty editor and a full-window title prompt; frontmatter is seeded only after the title is committed. Never auto-fill a draft with template scaffolding before the writer has decided what they're writing about.
8. **One typeface family for prose, one for display.** Hack for everything 14pt and below. Space Grotesk for everything 24pt and above. Below 24px the type stays in Hack, never mid-line mixed.
9. **Don't fake polish you haven't earned.** Known stubs stay marked as stubs. The Publish flow's missing modal is acknowledged in CLAUDE.md, not papered over with a toast.

## Accessibility & Inclusion

Best effort. No formal WCAG target; Chris's own eyes and hands are the calibration. That said:

- **Reduced motion is the default state**, matching the marketing site. There is essentially no motion in the app today, and there should not be any added without a reason that survives the YAGNI rule and respects `prefers-reduced-motion`.
- **Color must not be the only carrier of meaning.** The phoenix-blue, signal-cyan, and forge-amber accents are decorative or interactive; status (dirty, saved, authenticated, error) is always conveyed by text first.
- **Keyboard reachability** for every command in the editor's status bar is desirable. Today these are tap labels with `GestureRecognizers`; promoting them to real focusable controls is on the table whenever the status bar is next touched.
- **Light and dark modes are both real**, not afterthoughts. Every color binding is `AppThemeBinding` against the Blue Fenix token set. Dark is the brand default; light is the writer's-daylight accommodation.
