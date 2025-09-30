# SiteUtils

Site utilities for the FPV documentation project.

## BuildNavigation

A .NET 9 console application that generates the navigation section for `mkdocs.yml` based on the file and folder structure of the `docs` folder.

### Features

- Scans the `docs` folder recursively
- Builds a hierarchical navigation structure
- Transforms file and folder names:
  - Removes leading numbers (e.g., `00_`, `10_`, etc.)
  - Replaces underscores with spaces
- Generates YAML format for the `nav` section
- Updates the `mkdocs.yml` file

### Requirements

- .NET 9.0 SDK

### Usage

From the repository root:

```bash
dotnet run --project SiteUtils/BuildNavigation/BuildNavigation.csproj
```

Or from the SiteUtils folder:

```bash
cd SiteUtils
dotnet run --project BuildNavigation/BuildNavigation.csproj
```

### Example

Given a file structure like:
```
docs/
  00_Дроны(Квадрокоптеры)/
    00_Общая_информация.md
    01_Модели/
      Betafpv/
        Meteor75_Pro.md
```

The tool will generate:
```yaml
nav:
  - Дроны(Квадрокоптеры):
    - Общая информация: 00_Дроны(Квадрокоптеры)/00_Общая_информация.md
    - Модели:
      - Betafpv:
        - Meteor75 Pro: 00_Дроны(Квадрокоптеры)/01_Модели/Betafpv/Meteor75_Pro.md
```

### Building

To build the solution:

```bash
cd SiteUtils
dotnet build
```

### Notes

- The tool automatically finds the repository root by looking for the `.git` folder
- If a `nav` section already exists in `mkdocs.yml`, it will be replaced
- Only `.md` files are included in the navigation
- Files and folders starting with `.` are ignored

## NameTransformer

A .NET 9 console application that transliterates Cyrillic file and folder names to Latin characters and updates all references in the repository.

### Features

- Accepts a path to a file or folder inside the `docs` directory
- Transliterates Cyrillic characters to Latin using ISO 9 (GOST 7.79 System A) standard
- Renames files and folders only if they contain Cyrillic characters
- Updates references in:
  - `mkdocs.yml` navigation section
  - All `.md` files in the `docs` folder

### Requirements

- .NET 9.0 SDK

### Usage

From the repository root:

```bash
dotnet run --project SiteUtils/NameTransformer/NameTransformer.csproj -- <path>
```

Where `<path>` is the path to a file or folder inside the `docs` directory.

### Example

```bash
# Transliterate a folder name
dotnet run --project SiteUtils/NameTransformer/NameTransformer.csproj -- docs/30_Полеты

# Result: docs/30_Полеты -> docs/30_Polety
```

### Transliteration Table

The tool uses ISO 9 transliteration standard:

| Cyrillic | Latin | Cyrillic | Latin |
|----------|-------|----------|-------|
| А/а | A/a | О/о | O/o |
| Б/б | B/b | П/п | P/p |
| В/в | V/v | Р/р | R/r |
| Г/г | G/g | С/с | S/s |
| Д/д | D/d | Т/т | T/t |
| Е/е | E/e | У/у | U/u |
| Ё/ё | Yo/yo | Ф/ф | F/f |
| Ж/ж | Zh/zh | Х/х | H/h |
| З/з | Z/z | Ц/ц | Ts/ts |
| И/и | I/i | Ч/ч | Ch/ch |
| Й/й | J/j | Ш/ш | Sh/sh |
| К/к | K/k | Щ/щ | Shh/shh |
| Л/л | L/l | Ъ/ъ | (removed) |
| М/м | M/m | Ы/ы | Y/y |
| Н/н | N/n | Ь/ь | (removed) |
| | | Э/э | E/e |
| | | Ю/ю | Yu/yu |
| | | Я/я | Ya/ya |

### Notes

- The tool validates that the path is inside the `docs` folder
- If the name doesn't contain Cyrillic characters, no changes are made
- All references in markdown files and `mkdocs.yml` are automatically updated
- The tool automatically finds the repository root by looking for the `.git` folder
