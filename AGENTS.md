# Agent instructions for the FPV knowledge base

## Repository

- A knowledge base about FPV drones, written in Russian. Published at https://get-the-manual.github.io/fpv/.
- Pages are Markdown files in `docs/`. The site is built with [Zensical](https://zensical.org/), the successor of Material for MkDocs. Its configuration is still `mkdocs.yml` (theme variant `classic`).
- The `build-site.yml` workflow runs `zensical build --strict --clean` on every push to `main` and publishes the result to GitHub Pages with `actions/deploy-pages` (the Pages source is "GitHub Actions", not a branch).
- `SiteUtils/` holds small .NET 9 helper tools. They are rarely needed; their rules are in `SiteUtils/AGENTS.md`.
- `scripts/` holds helper scripts (PowerShell 7 and Python). Keep scripts out of `docs/`: Zensical publishes every file there.

## Content rules

### Files and folders

- Use only Latin letters, digits, `_`, `-` and `.` in file and folder names: English words or transliterated Russian (`Obshhaya_informatsiya.md`). No spaces, no Cyrillic. To transliterate a Cyrillic name, use NameTransformer (see `SiteUtils/AGENTS.md`).
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

- Link to other pages with a relative path to the `.md` file, for example `[текст](../20_VTX/OSD.md)`. A strict build fails on a broken link or anchor.
- Encode spaces in link targets as `%20`, or better, do not create files with spaces.
- Cross-page anchors use mkdocs-autorefs: define an anchor with `[](){#anchor_id}` and link to it with `[текст][anchor_id]`.
- In raw HTML (for example `<video src="...">` or `<model-viewer src="...">`), write paths relative to the `.md` file, as in Markdown links. Zensical rewrites them for the page URL (`.../Page/`). This differs from MkDocs, which left such paths unchanged. A strict build does not check paths in raw HTML, so open the page and check the file loads.
- When two pages cover related topics, link them both ways.

### Facts and sources

- Check technical facts against an official source (Betaflight, ExpressLRS, EdgeTX docs, firmware source code, the manufacturer's site). Add the link.
- Say which firmware version the values apply to. Parameters change between versions (for example, Betaflight Anti-Gravity changed in 4.3 and 4.4).
- Do not paste unchecked AI-generated text. Remove tracking parameters such as `utm_source=chatgpt.com` and placeholder links.
- Write in Russian with correct grammar. Keep English for product names, parameter names and UI labels.

### Media and large files

- Keep images reasonably small for the web. Save photo-like screenshots as JPG, not PNG.
- Before you commit new or changed images, run `python scripts/optimize_images.py <files or folders>` (needs `pip install pillow numpy pyoxipng imagequant`). Without `--apply` it is a dry run; with `--apply` it rewrites files in place. It keeps names, formats and pixel sizes: PNG is recompressed losslessly or, for files over 300 KB, reduced to a 256-colour palette when that saves at least 40 % at PSNR ≥ 38 dB; JPEG is re-encoded at quality 85 when PSNR stays ≥ 38 dB. A file changes only if it gets at least 10 % smaller. Look at the changed images before you commit.
- Store external images and animations in the repository instead of hotlinking them, so a page does not depend on another site. Credit the source on the page.
- Do not add long videos (more than about a minute, or with sound) to the repository. Link to YouTube or similar instead. Media stay in plain Git without Git LFS; issue #16 explains why.
- Do not add animated GIFs. Convert every animation to MP4 (see "Add an animation" below). An MP4 is usually 5–20 times smaller than the same GIF.
- Embed an animation as a looping video, not as an image:

  ```html
  <video src="Name.mp4" autoplay loop muted playsinline width="W" height="H"></video>
  ```

  - `autoplay loop muted` make it behave like a GIF. Browsers start a video on their own only when it is muted.
  - `playsinline` keeps the video inside the page on iPhone. Do not add `controls`.
  - `width` and `height` are the real size of the video. The browser reserves the space, so the page does not jump while the video loads.
  - `src` is relative to the `.md` file (see "Links"); a video in the same folder needs no prefix.
- No empty bars around the picture. MP4 has no transparency, so transparent GIF borders turn into white bars, which stand out in dark mode. Crop transparent borders and solid black or white bars, so the video holds only the picture.
- `.gitattributes` normalizes line endings and marks media formats as binary. Do not convert line endings by hand.

## Workflow

### Add or change a page

1. Create or edit the `.md` file in the right `docs/` folder.
2. Add or update its entry in `nav:` in `mkdocs.yml`.
3. Build the site in strict mode (see below) and fix every warning: CI uses `--strict` too, so any warning stops the deployment.
4. Commit the page, its assets and `mkdocs.yml` together.

### Add an animation

`scripts/Convert-GifToMp4.ps1` converts GIFs to MP4. It needs PowerShell 7 and FFmpeg (`ffmpeg` and `ffprobe` on `PATH`, or pass `-FfmpegDir` or set `FFMPEG_DIR`).

1. Put the GIF in the folder of the page and link it as usual: `![](Name.gif)`.
2. Run `pwsh scripts/Convert-GifToMp4.ps1 docs/<folder>/Name.gif -UpdateLinks`. The script:
   - crops transparent borders;
   - writes `Name.mp4` (H.264, `yuv420p`, `faststart`, no audio) next to the GIF;
   - replaces `![...](Name.gif)` in the pages of that folder with the `<video>` tag and the real `width` and `height`.
3. If the script warns about possible solid bars, look at a frame of the video. If they are bars, run it again with `-CropSolidBars`. Dark scenes can look like black bars, so always check the frame first.
4. Look at one frame of each video, zoomed in at the edges: the picture must fill it, with no bars or thin frame lines on any side. If a 1–3 px grey or dark frame remains (cropdetect misses soft borders), run the script again with `-TrimEdges 3` (more if needed).
5. When no page links to the GIF any more, remove it with `git rm`. Search the whole repository for the file name first.
6. Build in strict mode and check the page in the browser, in both light and dark mode.

### Move or rename files

1. Move or rename the files (`git mv`).
2. Update every relative link that points to them, in all pages, and the paths in `nav:`.
3. Build in strict mode.

Moving a page changes its URL on the site; external links to the old URL break. Do it only when needed.

### Check the build locally

```bash
pip install "zensical~=0.1.3"
zensical build --strict
zensical serve   # local preview
```

- On Windows, Zensical 0.1.3 fails in the git-revision-date plugin with a path error. Build with the plugin turned off: set `FPV_GIT_DATES=false` (PowerShell: `$env:FPV_GIT_DATES = 'false'`). Pages then have no dates; CI on Linux builds them with dates.
- The git-revision-date plugin warns about moved files until the move is committed. A strict build of uncommitted moves can fail only for that reason.
- Pin the same Zensical version as `build-site.yml`. Zensical is on the 0.x line, so check the build before you raise the version.

## When suggesting changes

- Prefer minimal, focused changes. Do not refactor working code without a reason.
- Explain changes that break URLs, tool behavior or the site structure.
