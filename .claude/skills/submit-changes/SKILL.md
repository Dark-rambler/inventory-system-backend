---
name: submit-changes
description: Show the user a summary of uncommitted changes for review, then commit if they approve.
---

Follow these steps exactly each time /submit is invoked:

## Step 1 — Show what changed

Run the following two commands in parallel and present the output to the user:
- `git status`
- `git diff HEAD --stat`

Then summarize in plain language: which files were added, modified, or deleted and what feature or fix they belong to.

## Step 2 — Ask for review

Say exactly this (nothing more):

> Here are the pending changes. Do you have any observations, or should I go ahead and commit?

Then wait for the user's response. Do NOT commit yet.

## Step 3 — Handle the response

**If the user has observations or requests changes:**
- Address every observation before proceeding.
- After all fixes are done, go back to Step 1 and repeat.

**If the user approves (says something like "looks good", "go ahead", "no observations", "commit it", etc.):**
- Follow the full git commit protocol from CLAUDE.md:
  1. Run `git status` and `git diff` to confirm staged/unstaged state.
  2. Stage only the relevant files by name (never `git add -A` blindly).
  3. Write a concise commit message focused on *why*, not *what*.
  4. Include the Co-Authored-By trailer.
  5. Run `git push origin develop` to push the commit to GitHub.
  6. Report the resulting commit hash and confirm the push to the user.
