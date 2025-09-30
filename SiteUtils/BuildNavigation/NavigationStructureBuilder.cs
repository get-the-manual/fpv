using System.Text.RegularExpressions;

namespace BuildNavigation;

public partial class NavigationStructureBuilder
{
    public static NavItem BuildFromFolder(string docsPath)
    {
        var rootItem = new NavItem { Name = "Root", IsDirectory = true };

        // Get all items in docs folder
        var items = Directory.GetFileSystemEntries(docsPath)
            .OrderBy(x => x)
            .ToList();

        foreach (var item in items)
        {
            var itemName = Path.GetFileName(item);

            // Skip hidden files/folders and index.md at root
            if (itemName.StartsWith("."))
                continue;

            if (File.Exists(item))
            {
                if (Path.GetExtension(item).ToLower() != ".md") continue;

                var relativePath = Path.GetRelativePath(docsPath, item).Replace("\\", "/");
                var displayName = GetDisplayName(Path.GetFileNameWithoutExtension(item));

                rootItem.Children.Add(new()
                {
                    Name = displayName,
                    Path = relativePath,
                    IsDirectory = false
                });
            }
            else if (Directory.Exists(item))
            {
                var dirNavItem = BuildDirectoryNavigation(item, docsPath);
                if (dirNavItem != null)
                    rootItem.Children.Add(dirNavItem);
            }
        }

        return rootItem;
    }

    private static NavItem? BuildDirectoryNavigation(string dirPath, string docsPath)
    {
        var dirName = Path.GetFileName(dirPath);
        var displayName = GetDisplayName(dirName);

        var navItem = new NavItem
        {
            Name = displayName,
            IsDirectory = true
        };

        // Get all items in directory
        var items = Directory.GetFileSystemEntries(dirPath)
            .OrderBy(x => x)
            .ToList();

        foreach (var item in items)
        {
            var itemName = Path.GetFileName(item);

            // Skip hidden files/folders
            if (itemName.StartsWith("."))
                continue;

            if (File.Exists(item))
            {
                if (Path.GetExtension(item).ToLower() != ".md") continue;

                var relativePath = Path.GetRelativePath(docsPath, item).Replace("\\", "/");
                var fileDisplayName = GetDisplayName(Path.GetFileNameWithoutExtension(item));

                navItem.Children.Add(new()
                {
                    Name = fileDisplayName,
                    Path = relativePath,
                    IsDirectory = false
                });
            }
            else if (Directory.Exists(item))
            {
                var subDirNavItem = BuildDirectoryNavigation(item, docsPath);
                if (subDirNavItem is { Children.Count: > 0 })
                    navItem.Children.Add(subDirNavItem);
            }
        }

        return navItem.Children.Count > 0 ? navItem : null;
    }

    [GeneratedRegex(@"^\d+_")]
    private static partial Regex LeadingNumbersRegex();

    private static string GetDisplayName(string name)
    {
        // Remove leading numbers and underscore (e.g., "00_" or "10_")
        name = LeadingNumbersRegex().Replace(name, "");

        // Replace underscores with spaces
        name = name.Replace("_", " ");

        return name;
    }

}