# Re-publish preserves the post path — design

**Issue:** [#48 "Ensure proper file name handling when editing a post"](https://github.com/BlueFenixProductions/PostXING/issues/48)
**Date:** 2026-06-10
**Status:** Approved design, pending implementation.
**Scope:** the filename/data-integrity bug only. The "material-design confirmation on mobile" half of #48 is split to a separate follow-up issue.

## Problem

Editing an already-published post and re-publishing it creates a **new, duplicate file dated today** instead of updating the original. The reporter saw one post land three times — `…06-03-…`, `…06-05-…`, `…06-06-…`.

Root cause (confirmed current on `develop`):

- `GitHubPublishService.PublishAsync` (`src/PostXING.GitHub/GitHubPublishService.cs:24,29`) unconditionally derives the target from *today*:
  ```csharp
  var publishDate = DateOnly.FromDateTime(_clock.GetUtcNow().UtcDateTime);
  var postPath = $"{site.PostsPrefix}{publishDate:yyyy-MM-dd}-{post.Slug.Value}.md";
  ```
- `EditorViewModel`'s publish calls `_publishService.PublishAsync(post, site, published, autoMerge: false)` (`src/PostXING.ViewModels/EditorViewModel.cs`) **without telling it the post already exists**, so the publish layer has no way to reuse the original path.

`SaveToBranchAsync` already preserves the opened path, so plain Save is *not* the culprit — the duplicate is born on Publish (and the Merge that follows it).

## Decisions

1. **Permalink policy — preserve the original path on re-publish.** When an already-published post (lives at `posts/{original-date}-{slug}.md`) is published again, write the edit back to that **same** path. Stable permalinks; no duplicate. First publish (from New or a draft) still uses today's date. This intentionally overrides the documented "date = publish time" convention *for re-publishes of already-published posts only* — permalink stability wins. (Operator decision.)
2. **Scope — bug fix now, confirmation UX separate.** The Android material-design confirmation is filed as a follow-up issue, not built here. (Operator decision.)
3. **Correction to the morning #48 comment:** `IGitHubGateway.DeleteFileAsync` *does* exist (`src/PostXING.GitHub/IGitHubGateway.cs:34`). It isn't needed here — preserve-in-place writes over the same path with no delete — but the original comment's "no delete-file op" claim is wrong and will be corrected on the issue.

## Design

### `GitHubPublishService.PublishAsync` — accept an existing path

Add an optional parameter `string? existingPostPath = null`. The path computation becomes:

```csharp
var postPath = existingPostPath ?? $"{site.PostsPrefix}{publishDate:yyyy-MM-dd}-{post.Slug.Value}.md";
```

When `null` → mint today's dated path (unchanged first-publish behavior). When supplied → write to that exact path (re-publish, original permalink preserved). The rendered document, PR, and merge flow are otherwise unchanged.

### `EditorViewModel` — detect "already published" and pass the path

On publish, compute:

```csharp
var existingPostPath =
    _handle.Source == PostSource.GitHub &&
    _handle.Identifier.StartsWith(site.PostsPrefix, StringComparison.Ordinal)
        ? _handle.Identifier
        : null;
```

and pass it to `PublishAsync`. A GitHub **draft** (`drafts/…`) or a **New** post yields `null` → first publish gets today's date (correct). Only a GitHub **post** already under `posts/…` triggers preservation.

### Slug-change edge

If the title (→ slug) changed on an already-published post, we still write to the **original** path — the filename keeps its original date *and* slug; only the frontmatter title updates. Permalink stability is the whole point, so a renamed title does not move (or duplicate) a published file.

### Branch-name collision (same-day re-publish)

The PR branch is currently `post/{slug}-{yyyyMMdd}` (today). Two re-publishes on the same day would collide on `CreateBranchAsync`. Make the branch name unique by extending it to second precision — `post/{slug}-{yyyyMMddHHmmss}` — so same-day re-publishes each get a fresh ephemeral branch. The branch is throwaway; only the **post path** must be stable.

## Testing (TDD)

`GitHubPublishService` is tested against `InMemoryGitHubGateway` with a fixed `TimeProvider`.

- **`existingPostPath = null`** → upserts `posts/{today}-{slug}.md` (locks existing first-publish behavior).
- **`existingPostPath = "posts/2026-06-03-foo.md"`** → upserts to **that** path, not today's (the fix).
- **`EditorViewModel` publish** → passes the identifier as `existingPostPath` when the handle is a GitHub post under `posts/`, and `null` for a draft / New post.

The branch-name uniqueness is covered incidentally by the fixed-clock tests asserting the computed branch; a same-day-collision regression test can assert two publishes don't reuse a branch name.

## Acceptance criteria

- Re-publishing an edited, already-published post updates the original `posts/{original-date}-{slug}.md` in place — no second dated file.
- First publish of a New post or a draft still creates `posts/{today}-{slug}.md`.
- A title/slug change on a published post does not move or duplicate the file.
- Existing publish/merge behavior and tests stay green.

## Out of scope (tracked separately)

- **Android material-design confirmation** (Snackbar on commit/publish/merge) — new follow-up issue.
- Deleting the originating `drafts/{slug}.md` when a draft is published (a pre-existing "Known gaps" item; unrelated to this duplicate bug).
