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
