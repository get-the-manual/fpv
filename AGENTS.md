# Agent instructions for the FPV knowledge base

## Repository

- A knowledge base about FPV drones, written in Russian. Published at https://get-the-manual.github.io/fpv/.
- Pages are Markdown files in `docs/`. The site is built with MkDocs + Material (`mkdocs.yml`).
- The `build-site.yml` workflow builds the site with `mkdocs gh-deploy --force --strict` on every push to `main` and publishes it to the `gh-pages` branch. Never edit `gh-pages` by hand.
- `--strict` turns every MkDocs warning into a failure: a broken relative link or a bad `nav:` path stops the deployment.
- `SiteUtils/` holds small .NET 9 helper tools (see the last section).

## Content rules

### Files and folders

- Use only Latin letters, digits, `_`, `-` and `.` in file and folder names: English words or transliterated Russian (`Obshhaya_informatsiya.md`). No spaces, no Cyrillic. Use NameTransformer to transliterate (see below).
- Use a numeric prefix (`10_`, `20_`) when the order inside a folder matters. Do not give two sibling folders the same number.
- One topic per page. Put images and other files for a page in the same folder as the page.

### Navigation

- The `nav:` section of `mkdocs.yml` is maintained by hand. Titles are in Russian and differ from file names.
- When you add, move or rename a page, update `nav:` yourself. Keep the order of sections in line with the folder numbers.
- Never run BuildNavigation: it rewrites the whole `nav:` section and loses the Russian titles.

### Page format

- Start every page with one H1 (`# Title`). Use H2/H3 for sections.
- Inside a paragraph, a line break is two spaces at the end of the line (MD009 is disabled in `.markdownlint.json` for this). `docs/FixMarkdownLineBreaks.ps1` adds the missing trailing spaces.
- Video links follow the pattern `[Video title. YouTube: Channel name](url)`.
- Product pages usually have a `[На сайте производителя](url)` link and a product image.

### Links

- Link to other pages with a relative path to the `.md` file, for example `[текст](../20_VTX/OSD.md)`. MkDocs checks these links.
- Encode spaces in link targets as `%20`, or better, do not create files with spaces.
- Cross-page anchors use mkdocs-autorefs: define an anchor with `[](){#anchor_id}` and link to it with `[текст][anchor_id]`.
- MkDocs does not rewrite paths inside raw HTML (for example `<model-viewer src="...">`). On the site such a path resolves from the page URL (`.../Page/`), not from the folder of the `.md` file.
- When two pages cover related topics, link them both ways.

### Facts and sources

- Check technical facts against an official source (Betaflight, ExpressLRS, EdgeTX docs, firmware source code, the manufacturer's site). Add the link.
- Say which firmware version the values apply to. Parameters change between versions (for example, Betaflight Anti-Gravity changed in 4.3 and 4.4).
- Do not paste unchecked AI-generated text. Remove tracking parameters such as `utm_source=chatgpt.com` and placeholder links.
- Write in Russian with correct grammar. Keep English for product names, parameter names and UI labels.

### Media and large files

- Keep images reasonably small for the web.
- Do not add large videos or GIFs to the repository until issue #16 (Git LFS or external hosting) is resolved. Link to YouTube or similar instead.
- `.gitattributes` normalizes line endings and marks media formats as binary. Do not convert line endings by hand.

## Workflow

### Add or change a page

1. Create or edit the `.md` file in the right `docs/` folder.
2. Add or update its entry in `nav:` in `mkdocs.yml`.
3. Build the site in strict mode (see below) and fix every warning.
4. Commit the page, its assets and `mkdocs.yml` together.

### Move or rename files

1. Move or rename the files (`git mv`).
2. Update every relative link that points to them, in all pages, and the paths in `nav:`.
3. Build in strict mode.

Moving a page changes its URL on the site; external links to the old URL break. Do it only when needed.

### Check the build locally

```bash
pip install mkdocs-material mkdocs-autorefs mkdocs-git-revision-date-localized-plugin
mkdocs build --strict
```

The git-revision-date plugin warns about moved files until the move is committed. A strict build of uncommitted moves can fail only for that reason.

## SiteUtils (C#)

- **NameTransformer** transliterates the name of one file or folder inside `docs/` and renames it. Then it replaces the old name with the new name in `mkdocs.yml` and in all `.md` files. The replacement is plain text, so review the diff.

  ```bash
  # from the repository root
  dotnet run --project SiteUtils/NameTransformer/NameTransformer.csproj -- docs/path/to/Имя_файла.md
  ```

  The mapping is a project-specific scheme, not a formal standard: `ё→yo`, `ж→zh`, `й→j`, `х→h`, `ц→ts`, `ч→ch`, `ш→sh`, `щ→shh`, `ы→y`, `э→e`, `ю→yu`, `я→ya`, `ъ`/`ь` dropped. Do not change it: existing file names depend on it.
- **BuildNavigation** is a legacy one-off tool. It is kept only as a buildable project. Do not run it (see Navigation).
- `build-siteutils.yml` builds the solution and test-runs both tools. It runs only on changes in `SiteUtils/` or in the workflow itself.

### Code style

Follow the existing code:

- .NET 9, top-level statements in `Program.cs`, file-scoped namespaces (`namespace NameTransformer;`).
- Four-space indentation, `var` for locals, one class per file, the file is named after the class.
- Static classes for helpers, `[GeneratedRegex]` for regular expressions.
- `Path.Combine` / `Path.GetRelativePath`; write paths with `/` in output and in `mkdocs.yml`.
- Find the repository root by walking up to the folder that contains `.git`.
- Print clear messages with `Console.WriteLine`; return 0 on success and 1 on error.

## When suggesting changes

- Prefer minimal, focused changes. Do not refactor working code without a reason.
- Explain changes that break URLs, tool behavior or the site structure.
