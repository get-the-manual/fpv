# Agent instructions for SiteUtils

Small .NET 9 console tools that help maintain the documentation in `docs/`. The general repository rules are in the root `AGENTS.md`.

## Tools

- **NameTransformer** transliterates the name of one file or folder inside `docs/` and renames it. Then it replaces the old name with the new name in `mkdocs.yml` and in all `.md` files. The replacement is plain text, so review the diff.

  ```bash
  # from the repository root
  dotnet run --project SiteUtils/NameTransformer/NameTransformer.csproj -- docs/path/to/Имя_файла.md
  ```

  The mapping is a project-specific scheme, not a formal standard: `ё→yo`, `ж→zh`, `й→j`, `х→h`, `ц→ts`, `ч→ch`, `ш→sh`, `щ→shh`, `ы→y`, `э→e`, `ю→yu`, `я→ya`, `ъ`/`ь` dropped. Do not change it: existing file names depend on it.
- **BuildNavigation** is a legacy one-off tool. It is kept only as a buildable project. Do not run it: it rewrites the whole `nav:` section of `mkdocs.yml` and loses the hand-written Russian titles.
- `.github/workflows/build-siteutils.yml` builds the solution and test-runs both tools. It runs only on changes in `SiteUtils/` or in the workflow itself.

## Build

```bash
dotnet build SiteUtils/SiteUtils.sln
```

## Code style

Follow the existing code:

- .NET 9, top-level statements in `Program.cs`, file-scoped namespaces (`namespace NameTransformer;`).
- Four-space indentation, `var` for locals, one class per file, the file is named after the class.
- Static classes for helpers, `[GeneratedRegex]` for regular expressions.
- `Path.Combine` / `Path.GetRelativePath`; write paths with `/` in output and in `mkdocs.yml`.
- Find the repository root by walking up to the folder that contains `.git`.
- Print clear messages with `Console.WriteLine`; return 0 on success and 1 on error.
