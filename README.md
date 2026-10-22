# Fili.ReadyTemplate

**Fili.ReadyTemplate 0.1.0** is a continuation of **ReadySET** (Ready-to-use Software
Engineering Templates), the set of HTML templates for software project documentation that Jason
Robbins published at readyset.tigris.org between 2003 and 2004. Tigris.org has since shut down and
ReadySET 0.9.3 was its last release. This repository keeps the templates available, brings the
pages up to current web standards, repairs the links that died with Tigris.org, and serves the
result as a static site with GitHub Pages.

The templates themselves are Jason Robbins' work under the ReadySET license (see below).
Fili.ReadyTemplate is maintained by this repository's owner and is not affiliated with or
endorsed by the ReadySET author.

The site is plain HTML and CSS with no build step and no JavaScript. `index.html` at the
repository root is the entry page; `templates/browse.html` lists every template.

| Folder | Contents |
|---|---|
| `templates/` | The templates themselves: proposal, SRS, use cases, design, QA plan, test cases, release notes, checklists and more, plus the `all-in-one.html` bundle and the stylesheets in `templates/css/` |
| `docs/` | The FAQ, upgrade how-to and recommended tools inherited from ReadySET, and the [Fili.ReadyTemplate 0.1.0 release notes](docs/release-notes-0-1-0.html) |
| `docs/readyset/` | The original ReadySET 0.9.0 to 0.9.3 release notes, kept as history and marked as such |

## What 0.1.0 changed

The template *content* is the ReadySET 0.9.3 original. Around it:

- **HTML5 pages.** Every page lost its XML prologue and Tigris DOCTYPE and gained
  `<!DOCTYPE html>`, `lang="en"`, a UTF-8 charset and a viewport meta tag. The inline
  `@import` style blocks became `<link>` tags to the three stylesheets.
- **New stylesheets.** `readyset.css` was rewritten on CSS custom properties with a readable
  single column, system fonts, responsive tables and a dark colour scheme that follows the
  operating system. `inst.css` is still the customisation layer the templates tell you to edit,
  and `print.css` still hides the sticky notes and footnote on paper. Every class name from
  ReadySET 0.9.3 is honoured, so documents made from the old templates render correctly.
- **No more frames.** The `frameset.html` / `frame-nav.html` pair and its JavaScript tree were
  replaced by `templates/browse.html`, a plain page that lists the templates by activity, by
  suggested sequence and alphabetically. A small navigation bar at the top of every page links
  back to it. That bar is the one addition inside each template: delete the
  `<nav class="sitenav">` element when you copy a template into your own project.
- **Links.** Everything that pointed at the dead `readyset.tigris.org` was repointed: the
  *words of wisdom* essays to the Internet Archive, downloads to this repository's zip, the
  glossary and home page to this site, the issue tracker and mailing list to this repository's
  issues. Eleven mistyped internal links and anchors were corrected. Placeholders such as
  `LINK-TO-DIAGRAM` are intentional blanks in the templates and were left alone.
- **ReadySET Pro.** The commercial companion to ReadySET is no longer sold: its site is a 2010
  snapshot whose checkout posts to a payment processor that closed in 2016. The sentence
  inviting readers to evaluate it was removed from the TODO box at the foot of every template.
  Mentions in the original author's prose and in the license file were left as written.

## Using the templates

Download the [zip of this repository](https://github.com/FiliArrochada/Fili.ReadyTemplate/archive/refs/heads/main.zip)
or clone it, copy the `templates` folder into your project, and edit the pages with any HTML or
text editor. Yellow TODO and TIP boxes are instructions to delete, text in ALL CAPS is to be
replaced, and example text carries a coloured rule on its right-hand edge. The
[FAQ](docs/faq.html) explains the conventions; `templates/css/inst.css` is where fonts, colours
and the show/hide toggles are customised.

## Publishing with GitHub Pages

The site is served from the root of the default branch. `.nojekyll` at the root tells Pages to
serve the files as they are rather than running them through Jekyll.

## License

The templates are copyright 2003-2004 Jason Robbins and are distributed under the BSD-style
license in [`templates/readyset-license.html`](templates/readyset-license.html). That page is
kept exactly as ReadySET shipped it, and it is also where the original project's history, its
author's contact details and the license FAQ live.

The license governs this repository under any name and cannot be dropped: copyright in a 2004
work runs for the author's lifetime plus seventy years, and the license is the only permission
anyone has to redistribute the templates at all. Its conditions are met here:

- Redistribution with or without modification is allowed, so the 0.1.0 changes are permitted.
- Every template still ends with its copyright footnote and a link to the license, and the
  license file itself is unchanged, as the license requires. Only a project's own filled-in
  documents may hide the footnote, which `inst.css` explains how to do.
- The ReadySET name must not be used to endorse or promote a derived product, which is why the
  continuation carries its own name and says everywhere that the author has not endorsed it.
