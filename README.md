# ReadySET 0.9.3

A mirror of **ReadySET** (Ready-to-use Software Engineering Templates) release 0.9.3, the set of
HTML templates for software project documentation originally published at readyset.tigris.org
in 2004. Tigris.org has since shut down, so this copy is kept here and served as a static site
with GitHub Pages.

The site is plain HTML, CSS and a little JavaScript with no build step. `index.html` at the
repository root is the entry page.

| Folder | Contents |
|---|---|
| `templates/` | The templates themselves: proposal, SRS, use cases, design, QA plan, test cases, release notes, checklists and more, plus the `all-in-one.html` bundle |
| `docs/` | ReadySET's own FAQ, upgrade how-to, recommended tools and release notes |

## Publishing with GitHub Pages

1. Push this repository to GitHub.
2. In the repository settings, open **Pages**, choose **Deploy from a branch**, and pick the
   default branch with the **/ (root)** folder.
3. The site appears at `https://<owner>.github.io/<repository>/` within a minute or two.

`.nojekyll` at the root tells Pages to serve the files as they are rather than running them
through Jekyll.

## License

The templates are copyright 2003-2004 Jason Robbins and are distributed under the BSD-style
license in [`templates/readyset-license.html`](templates/readyset-license.html). Redistribution is
permitted provided that file and the copyright footnote in each template are kept, and the
ReadySET name is not used to endorse derived products.
