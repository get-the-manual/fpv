using System.Text;
using System.Text.RegularExpressions;

namespace BuildNavigation;

class Program
{
    static void Main(string[] args)
    {
        string repoRoot = FindRepositoryRoot();
        string docsPath = Path.Combine(repoRoot, "docs");
        string mkdocsPath = Path.Combine(repoRoot, "mkdocs.yml");

        if (!Directory.Exists(docsPath))
        {
            Console.WriteLine($"Error: docs folder not found at {docsPath}");
            return;
        }

        if (!File.Exists(mkdocsPath))
        {
            Console.WriteLine($"Error: mkdocs.yml not found at {mkdocsPath}");
            return;
        }

        Console.WriteLine($"Scanning docs folder: {docsPath}");
        var navStructure = BuildNavigationStructure(docsPath);

        Console.WriteLine("Generating navigation YAML...");
        var navYaml = GenerateNavYaml(navStructure);

        Console.WriteLine("Updating mkdocs.yml...");
        UpdateMkdocsYml(mkdocsPath, navYaml);

        Console.WriteLine("Navigation section generated successfully!");
    }

    static string FindRepositoryRoot()
    {
        string? currentDir = Directory.GetCurrentDirectory();
        
        while (currentDir != null)
        {
            if (Directory.Exists(Path.Combine(currentDir, ".git")))
            {
                return currentDir;
            }
            currentDir = Directory.GetParent(currentDir)?.FullName;
        }
        
        throw new Exception("Could not find repository root (no .git folder found)");
    }

    static NavItem BuildNavigationStructure(string docsPath)
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
                if (Path.GetExtension(item).ToLower() == ".md")
                {
                    var relativePath = Path.GetRelativePath(docsPath, item).Replace("\\", "/");
                    var displayName = GetDisplayName(Path.GetFileNameWithoutExtension(item));
                    
                    rootItem.Children.Add(new NavItem
                    {
                        Name = displayName,
                        Path = relativePath,
                        IsDirectory = false
                    });
                }
            }
            else if (Directory.Exists(item))
            {
                var dirNavItem = BuildDirectoryNavigation(item, docsPath);
                if (dirNavItem != null)
                {
                    rootItem.Children.Add(dirNavItem);
                }
            }
        }

        return rootItem;
    }

    static NavItem? BuildDirectoryNavigation(string dirPath, string docsPath)
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
                if (Path.GetExtension(item).ToLower() == ".md")
                {
                    var relativePath = Path.GetRelativePath(docsPath, item).Replace("\\", "/");
                    var fileDisplayName = GetDisplayName(Path.GetFileNameWithoutExtension(item));
                    
                    navItem.Children.Add(new NavItem
                    {
                        Name = fileDisplayName,
                        Path = relativePath,
                        IsDirectory = false
                    });
                }
            }
            else if (Directory.Exists(item))
            {
                var subDirNavItem = BuildDirectoryNavigation(item, docsPath);
                if (subDirNavItem != null && subDirNavItem.Children.Count > 0)
                {
                    navItem.Children.Add(subDirNavItem);
                }
            }
        }

        return navItem.Children.Count > 0 ? navItem : null;
    }

    static string GetDisplayName(string name)
    {
        // Remove leading numbers and underscore (e.g., "00_" or "10_")
        name = Regex.Replace(name, @"^\d+_", "");
        
        // Replace underscores with spaces
        name = name.Replace("_", " ");
        
        return name;
    }

    static string GenerateNavYaml(NavItem root)
    {
        var sb = new StringBuilder();
        sb.AppendLine("nav:");
        
        foreach (var child in root.Children)
        {
            AppendNavItem(sb, child, 1);
        }
        
        return sb.ToString();
    }

    static void AppendNavItem(StringBuilder sb, NavItem item, int level)
    {
        string indent = new string(' ', level * 2);
        
        if (item.IsDirectory)
        {
            sb.AppendLine($"{indent}- {item.Name}:");
            foreach (var child in item.Children)
            {
                AppendNavItem(sb, child, level + 1);
            }
        }
        else
        {
            sb.AppendLine($"{indent}- {item.Name}: {item.Path}");
        }
    }

    static void UpdateMkdocsYml(string mkdocsPath, string navYaml)
    {
        var lines = File.ReadAllLines(mkdocsPath).ToList();
        
        // Find if nav section already exists
        int navStartIndex = -1;
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Trim().StartsWith("nav:"))
            {
                navStartIndex = i;
                break;
            }
        }
        
        // Remove existing nav section if it exists
        if (navStartIndex >= 0)
        {
            int navEndIndex = navStartIndex + 1;
            while (navEndIndex < lines.Count && 
                   (lines[navEndIndex].StartsWith("  ") || lines[navEndIndex].StartsWith("\t") || string.IsNullOrWhiteSpace(lines[navEndIndex])))
            {
                navEndIndex++;
            }
            lines.RemoveRange(navStartIndex, navEndIndex - navStartIndex);
        }
        
        // Add new nav section at the end
        lines.Add("");
        lines.AddRange(navYaml.TrimEnd().Split('\n'));
        
        File.WriteAllText(mkdocsPath, string.Join(Environment.NewLine, lines));
    }
}

class NavItem
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public bool IsDirectory { get; set; }
    public List<NavItem> Children { get; set; } = new List<NavItem>();
}
