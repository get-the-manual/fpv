# Copilot instructions for FPV Documentation Repository

## Repository Purpose
This repository hosts a comprehensive knowledge base about FPV (First Person View) drones, published at https://get-the-manual.github.io/fpv/. The content is primarily in Russian and covers topics for beginners and enthusiasts in the FPV drone hobby.

## Project Structure

### Documentation (`docs/`)
- Main content directory containing all documentation in Markdown format
- Organized hierarchically by topic (drones, controllers, goggles, simulators, flight techniques, etc.)
- Primarily Russian language content with some English terms
- Uses MkDocs with Material theme for site generation
- Images are stored alongside markdown files

### Site Utilities (`SiteUtils/`)
Two .NET 9 console applications for maintaining the documentation:

1. **BuildNavigation** - Generates the navigation section for `mkdocs.yml` based on the `docs/` folder structure
2. **NameTransformer** - Transliterates Cyrillic file/folder names to Latin characters and updates all references

## Documentation Guidelines

### Content Structure
- Use descriptive folder and file names
- Prefix with numbers for ordering (e.g., `00_`, `10_`, `20_`)
- Use underscores for spaces in file/folder names (transformed to spaces in navigation)
- Keep one topic per markdown file
- Store images in the same directory as the markdown file

### Markdown Conventions
- Use Markdown standard formatting
- Include descriptive headers (H1 for page title, H2-H3 for sections)
- Use relative paths for images: `![](image.png)`
- Include links to external resources (videos, articles, product pages)
- Keep language consistent (primarily Russian)

### When Modifying Documentation
- After adding/renaming/moving files, run BuildNavigation to update `mkdocs.yml`
- Use NameTransformer when transliterating Cyrillic names to ensure all references are updated
- Verify links still work after structural changes

## C# SiteUtils Development

### General Guidelines
- Target framework: .NET 9
- Follow SOLID principles
- Keep each utility focused on a single responsibility
- Use console applications for automation tasks

### Code Style
- Four-space indentation
- Use `namespace` blocks (no file-scoped namespaces)
- Place `using` directives at the top of files
- Use `var` for local variable declarations
- One class per file, file name matches class name
- Use static classes for utility functions
- Keep code buildable and compile-ready

### Naming Conventions
- PascalCase for class names, methods, and properties
- camelCase for local variables and parameters
- Descriptive names that reflect purpose

### Error Handling
- Validate inputs and provide clear error messages
- Use `Console.WriteLine` for user feedback
- Return appropriate exit codes (0 for success, 1 for errors)

### File Operations
- Use `Path.Combine` for path construction
- Use `Path.GetRelativePath` for relative paths
- Normalize path separators (`Replace("\\", "/")` for cross-platform compatibility)
- Find repository root by looking for `.git` folder

### Patterns to Follow
- See `PathHelper.cs` for repository root detection
- See `NavigationStructureBuilder.cs` for recursive directory traversal
- See `CyrillicTransliterator.cs` for character mapping with dictionaries
- Use regular expressions for pattern matching (e.g., `LeadingNumbersRegex`)

### What NOT to Change
- Existing tool behavior and output formats
- MkDocs configuration structure
- Transliteration mapping (ISO 9 / GOST 7.79 System A)

## Build and Test

### Building SiteUtils
```bash
cd SiteUtils
dotnet build
```

### Running Tools
```bash
# From repository root
dotnet run --project SiteUtils/BuildNavigation/BuildNavigation.csproj
dotnet run --project SiteUtils/NameTransformer/NameTransformer.csproj -- <path>
```

### Testing Documentation Site
```bash
# Install MkDocs dependencies (if not already installed)
pip install mkdocs mkdocs-material mkdocs-gen-nav-plugin mkdocs-autorefs

# Serve locally
mkdocs serve
```

## Repository Maintenance

### When Adding New Documentation
1. Create markdown file in appropriate `docs/` subdirectory
2. Run BuildNavigation to update `mkdocs.yml`
3. Test locally with `mkdocs serve`
4. Commit both the new file and updated `mkdocs.yml`

### When Modifying File Structure
1. Move/rename files as needed
2. Run BuildNavigation to regenerate navigation
3. If transliterating Cyrillic names, use NameTransformer
4. Verify all links work
5. Commit all changes

### Git Workflow
- The repository uses GitHub Pages for deployment
- CI/CD workflows are in `.github/workflows/`
- `build-site.yml` - Builds and deploys the MkDocs site
- `build-siteutils.yml` - Builds the C# utilities

## Special Considerations

### Cyrillic Support
- The repository contains extensive Russian content
- Use NameTransformer tool for systematic transliteration
- Don't manually transliterate, as it won't update references

### MkDocs Configuration
- Main config file: `mkdocs.yml`
- Navigation section is generated by BuildNavigation tool
- Manual edits to `nav:` section will be overwritten

### Images and Assets
- Store images alongside markdown files
- Use descriptive names for images
- Keep image sizes reasonable for web viewing

## Code Generation Preferences

### When Generating C# Code
- Follow existing patterns in SiteUtils
- Keep it simple and maintainable
- Add comments only for complex logic
- Include error handling and validation
- Make tools user-friendly with clear messages

### When Generating Documentation
- Match the existing documentation style
- Use proper Russian grammar and terminology
- Include relevant external links
- Organize content logically
- Keep files focused on specific topics

### When Suggesting Changes
- Prefer minimal, surgical changes
- Don't refactor working code unnecessarily
- Explain breaking changes
- Consider impact on existing documentation structure
- Test changes with the provided tools
