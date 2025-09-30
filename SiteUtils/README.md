# SiteUtils

This folder contains utility applications for managing the FPV documentation site.

## BuildNavigation

A .NET 9 console application that automatically generates the navigation section (`nav`) in `mkdocs.yml` based on the structure and content of the `docs` folder.

### Usage

From the repository root:

```bash
cd SiteUtils/BuildNavigation
dotnet run
```

The application will:
1. Scan all `.md` files in the `docs` folder
2. Generate a hierarchical navigation structure
3. Format navigation menu item names:
   - Remove leading numbers (e.g., `00_`, `10_`)
   - Replace underscores with spaces
4. Update the `nav` section in `mkdocs.yml`

### Example

A file named `00_Дроны(Квадрокоптеры)/00_Общая_информация.md` will appear in the navigation as:
- Folder: "Дроны(Квадрокоптеры)"
- Menu item: "Общая информация"

### Requirements

- .NET 9 SDK
