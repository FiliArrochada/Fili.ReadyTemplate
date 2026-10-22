# Fili.ReadyTemplate

**Fili.ReadyTemplate** is a continuation of **ReadySET** (Ready-to-use Software Engineering
Templates), the set of HTML templates for software project documentation that Jason Robbins
published at readyset.tigris.org between 2003 and 2004. Tigris.org has since shut down and
ReadySET 0.9.3 was its last release. This repository keeps the templates available, brings the
pages up to current web standards, repairs the links that died with Tigris.org, presents them in
more than one language, and serves the result with GitHub Pages at
`https://filiarrochada.github.io/Fili.ReadyTemplate/`.

The templates themselves are Jason Robbins' work under the ReadySET license (see below).
Fili.ReadyTemplate is maintained by this repository's owner and is not affiliated with or
endorsed by the ReadySET author.

## Layout

```
Directory.Build.props            shared block + this repo's settings (version, publisher)
Directory.Packages.props         Central Package Management: every package version lives here
Fili.ReadyTemplate.sln           classic solution; Solution Items and Pipelines folders
global.json                      opts `dotnet test` into Microsoft.Testing.Platform (xunit v3)

src/Fili.ReadyTemplate           the Blazor WebAssembly site (net10.0)
  wwwroot/templates              the templates: plain HTML, standalone, unchanged since 0.1.0
  wwwroot/docs                   FAQ, HOWTO-upgrade, recommended tools, release notes
  wwwroot/docs/readyset          the original ReadySET 0.9.0 to 0.9.3 release notes, kept as history
  wwwroot/content                fragments the shell renders, such as the 2004 project overview
  Resources/Strings*.resx        the shell's strings, English (default) and Portuguese
tst/Fili.ReadyTemplate.UnitTests xunit v3 tests over the catalog, the parser and the string tables
.github/workflows/pages.yml      tests, publishes and deploys the site to GitHub Pages
```

## How the site works

The Blazor app is a thin **shell** around static content. It renders the navigation bar, the
home page, the browse page and the language switch from its own string tables, and it fetches
each template or documentation page as a static file, strips that file's own navigation bar,
rebases its relative links, and shows the body inside the shell. The templates therefore stay
exactly what ReadySET meant them to be: self-contained HTML files that open, print and copy
without the site. Opened directly, `templates/proposal.html` renders with its own navigation bar;
opened through the site, that bar is replaced by the localized one.

**Language.** The culture is decided before .NET starts, in `wwwroot/js/app.js`: the stored
preference if there is one, else the first browser language the site has strings for, else
English. It is passed to `Blazor.start` so the runtime loads only that culture's satellite
assembly and ICU shard. Switching language stores the choice and reloads the page.

**Translated content.** A translation of a static page lives in a culture-named folder beside
the original: `templates/pt/proposal.html` translates `templates/proposal.html`, and
`docs/pt/faq.html` translates `docs/faq.html`. The shell tries the culture's copy first and falls
back to the English original with a notice. A translated template reaches the shared stylesheets
with `../css/`, so it also works as a standalone file when the `templates` folder is copied into
a project. No template has been translated yet; the mechanism is in place.

**Adding a language.** Add `Resources/Strings.<code>.resx` with every key from `Strings.resx`,
add the code to `CultureService.Supported` and to the `supported` list in `wwwroot/js/app.js`,
and add a `Lang_<code>` display name to every string table. The unit tests fail until all four
agree.

## Building and running

```powershell
dotnet build Fili.ReadyTemplate.sln
dotnet test  Fili.ReadyTemplate.sln
dotnet run --project src/Fili.ReadyTemplate          # dev server; the URL is printed
dotnet publish src/Fili.ReadyTemplate -c Release -o publish   # the Pages artifact is publish/wwwroot
```

The WebAssembly build needs the `wasm-tools` workload once: `dotnet workload install wasm-tools`.
The dev server answers a missing path that has a file extension with a real 404 rather than
the app, so deep links such as `/templates/x.html` open the raw file locally; on GitHub Pages,
`404.html` is the app and any path works.

## Deployment

`.github/workflows/pages.yml` runs on every push to `main`: it tests, publishes, rewrites
`<base href>` to `/Fili.ReadyTemplate/`, copies `index.html` to `404.html` so deep links reach
the app, and deploys `publish/wwwroot` with `actions/deploy-pages`. The repository's Pages
source must be set to **GitHub Actions** (Settings, Pages) for the deploy step to succeed.

## Using the templates

Download the [zip of this repository](https://github.com/FiliArrochada/Fili.ReadyTemplate/archive/refs/heads/main.zip)
or clone it, copy `src/Fili.ReadyTemplate/wwwroot/templates` into your project, and edit the
pages with any HTML or text editor. Yellow TODO and TIP boxes are instructions to delete, text in
ALL CAPS is to be replaced, and example text carries a coloured rule on its right-hand edge. The
[FAQ](src/Fili.ReadyTemplate/wwwroot/docs/faq.html) explains the conventions;
`templates/css/inst.css` is where fonts, colours and the show/hide toggles are customised. Delete
the `<nav class="sitenav">` line at the top of a copied template; it is the site's, not yours.

## Release history

- **0.2.0** (2026-09-16): the Blazor WebAssembly shell, English and Portuguese, per-culture
  content folders with fallback, `src/` + `tst/` layout, unit tests, GitHub Actions deployment.
  See [the release notes](src/Fili.ReadyTemplate/wwwroot/docs/release-notes-0-2-0.html).
- **0.1.0** (2026-09-16): HTML5 pages, new stylesheets with dark mode, frames replaced by a
  browse page, dead links repaired, ReadySET Pro references removed.
  See [the release notes](src/Fili.ReadyTemplate/wwwroot/docs/release-notes-0-1-0.html).
- **ReadySET 0.9.0 to 0.9.3** (2003-2004): the original project's notes, under
  `wwwroot/docs/readyset/`.

## License

The templates are copyright 2003-2004 Jason Robbins and are distributed under the BSD-style
license in [`templates/readyset-license.html`](src/Fili.ReadyTemplate/wwwroot/templates/readyset-license.html).
That page is kept exactly as ReadySET shipped it, and it is also where the original project's
history, its author's contact details and the license FAQ live.

The license governs the templates under any name and cannot be dropped: copyright in a 2004
work runs for the author's lifetime plus seventy years, and the license is the only permission
anyone has to redistribute the templates at all. Its conditions are met here:

- Redistribution with or without modification is allowed, so the changes around the templates
  are permitted.
- Every template still ends with its copyright footnote and a link to the license, and the
  license file itself is unchanged, as the license requires. Only a project's own filled-in
  documents may hide the footnote, which `inst.css` explains how to do.
- The ReadySET name must not be used to endorse or promote a derived product, which is why the
  continuation carries its own name and says everywhere that the author has not endorsed it.

The site code in `src/` and `tst/` is FiliArrochada's and is not covered by that license.
