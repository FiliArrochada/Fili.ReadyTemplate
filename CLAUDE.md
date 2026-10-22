# CLAUDE.md

Guidance for Claude Code working in this repository. The workspace `CLAUDE.md` one level up still
applies, in particular **never commit**: leave changes in the working tree, and when asked for a
"git script" write one for the human to run.

## What this is

**Fili.ReadyTemplate** is a Blazor WebAssembly site that presents the ReadySET software
engineering document templates (Jason Robbins, 2003-2004) in more than one language and is
deployed to GitHub Pages at `https://filiarrochada.github.io/Fili.ReadyTemplate/`. The README
explains the shape; this file records the constraints that are not obvious from the code.

## Commands

```powershell
dotnet build Fili.ReadyTemplate.sln
dotnet test  Fili.ReadyTemplate.sln                                  # xunit v3 on Microsoft.Testing.Platform
dotnet test  Fili.ReadyTemplate.sln --filter-method "*Rebased*"      # MTP filter syntax, not VSTest's
dotnet run --project src/Fili.ReadyTemplate                          # dev server
dotnet publish src/Fili.ReadyTemplate -c Release -o publish          # Pages artifact = publish/wwwroot
```

`global.json` opts `dotnet test` into Microsoft.Testing.Platform; the .NET 10 SDK refuses to run
xunit v3 through VSTest, and on SDK 10.0.400 the opt-in is `global.json` (`"test": {"runner": …}`),
not `dotnet.config`. Do not pass `--nologo` to `dotnet test` in this mode: it is forwarded to the
test executable, which rejects it and reports "Zero tests ran, error: 1" with no other clue. The
WebAssembly build needs `dotnet workload install wasm-tools` once.

## The two halves, and the line between them

- **Templates and docs are content, not code.** Everything under `wwwroot/templates` and
  `wwwroot/docs` is a standalone HTML file that must keep working when copied out of the site:
  its own `<head>`, its own `<nav class="sitenav">`, its copyright footnote. The shell strips the
  nav and rebases links at render time; it never needs the files to change. Do not turn templates
  into Razor components and do not add markup to them beyond that one `sitenav` line.
- **The shell is code.** Layout, home, browse, the language switch and the content viewer live in
  `src/Fili.ReadyTemplate`. All of its visible text comes from `Resources/Strings*.resx` through
  `IStringLocalizer<Strings>`; a hard-coded English string in a `.razor` file is a defect.

## Localization mechanics

- The culture is fixed **before .NET starts**, in `wwwroot/js/app.js`, and passed to
  `Blazor.start({ applicationCulture })`. The runtime loads that culture's satellite assembly and
  ICU shard at boot and cannot switch afterwards, which is why `CultureService.SetCultureAsync`
  stores the choice and reloads. `InvariantGlobalization` must stay `false` or no translation is
  ever seen.
- Four places must agree on the supported cultures: `CultureService.Supported`, the `supported`
  array in `app.js`, a `Strings.<code>.resx` with every key, and a `Lang_<code>` entry in every
  string table. `ResourceTests` and `ContentPathsTests` fail when they drift.
- Translated **content** lives in a culture folder beside the original (`templates/pt/x.html`,
  `docs/pt/x.html`, `docs/readyset/pt/x.html`). `ContentPaths.Candidates` encodes that rule;
  `ContentLoader` tries the culture copy, then the English original, and flags the fallback so
  the viewer can say so. A translated template links its stylesheets with `../css/`.

## Traps already fallen into

- **`<base href="/">` rebases every relative link in fetched content to the site root.** A
  template's `href="plan.html"` would navigate to `/plan.html`. `ContentParser.RebaseLinks`
  rewrites `href`/`src` against the content's own folder; fragment-only links become
  `contentPath#fragment`. Keep it if the base href ever changes.
- **A missing static file is not always a 404.** GitHub Pages answers with `404.html`, which is
  the app; the loader recognises the host page by `id="app"` (`ContentParser.SpaMarker`) and
  treats it as "not found". The dev server, by contrast, returns a real 404 for paths with a file
  extension and never serves the app for them, so `/templates/x.html` deep links open the raw file
  locally. Neither is a bug.
- **`@code` cannot be a Razor loop variable** (it is the directive), and a component cannot be
  referenced by a namespace-qualified tag; add a `@using` to `_Imports.razor` instead.
- **The Pages source must be "GitHub Actions".** With "Deploy from a branch" the workflow's deploy
  step fails and the old static site keeps being served. The workflow rewrites `<base href>` to
  `/Fili.ReadyTemplate/` and copies `index.html` to `404.html`; the app itself is built with
  `<base href="/">` so it runs locally unchanged.

## License constraints

The templates are under the ReadySET license (`templates/readyset-license.html`), which allows
modification but requires the license file to stay as shipped, the copyright footnote to stay in
every template, and the ReadySET name not to be used to endorse the derived product. The body of
`readyset-license.html` must remain byte-identical to the 2004 original; only its `<head>` and
`sitenav` line differ, like every other page. Every page that names this project says it is not
endorsed by the ReadySET author. Keep both.

## Release notes and versions

The version lives once, in `Directory.Build.props`, and reaches the navigation bar through
`AppInfo.Version`. A release adds `wwwroot/docs/release-notes-<x>-<y>-<z>.html` in the shape of
the existing ones and updates the two links to the current notes (layout and home page) plus the
static pages' `sitenav` labels. ReadySET's own 0.9.x notes stay under `docs/readyset/`, each with
its "historical document" notice.
