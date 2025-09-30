using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BuildNavigation;

class Program
{
    static void Main(string[] args)
    {
        // Determine the repository root directory (parent of SiteUtils)
        var currentDir = Directory.GetCurrentDirectory();
        var repoRoot = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
        
        var docsPath = Path.Combine(repoRoot, "docs");
        var mkdocsPath = Path.Combine(repoRoot, "mkdocs.yml");
        
        if (!Directory.Exists(docsPath))
        {
            Console.WriteLine($"Error: docs folder not found at {docsPath}");
            return;
        }
        
        if (!File.Exists(mkdocsPath))
        {
            Console.WriteLine($"Error: mkdocs.yml file not found at {mkdocsPath}");
            return;
        }
        
        Console.WriteLine($"Scanning docs folder: {docsPath}");
        Console.WriteLine($"mkdocs.yml location: {mkdocsPath}");
        
        // Build navigation structure
        var navItems = BuildNavigationStructure(docsPath);
        
        // Read and update mkdocs.yml
        UpdateMkDocsYaml(mkdocsPath, navItems);
        
        Console.WriteLine("Navigation section has been successfully generated!");
    }
    
    static List<object> BuildNavigationStructure(string docsPath)
    {
        var navItems = new List<object>();
        
        // Process index.md first if it exists
        var indexPath = Path.Combine(docsPath, "index.md");
        if (File.Exists(indexPath))
        {
            navItems.Add(new Dictionary<string, string>
            {
                { "Главная", "index.md" }
            });
        }
        
        // Get all directories and files in docs folder
        var entries = Directory.GetFileSystemEntries(docsPath)
            .Select(e => new FileInfo(e))
            .Where(f => f.Name != "index.md") // Skip index.md as it's already added
            .OrderBy(f => f.Name)
            .ToList();
        
        foreach (var entry in entries)
        {
            if (entry.Attributes.HasFlag(FileAttributes.Directory))
            {
                // Process directory
                var dirItem = ProcessDirectory(entry.FullName, docsPath);
                if (dirItem != null)
                {
                    navItems.Add(dirItem);
                }
            }
            else if (entry.Extension.Equals(".md", StringComparison.OrdinalIgnoreCase))
            {
                // Process markdown file
                var fileName = entry.Name;
                var displayName = FormatDisplayName(Path.GetFileNameWithoutExtension(fileName));
                var relativePath = Path.GetRelativePath(docsPath, entry.FullName).Replace("\\", "/");
                
                navItems.Add(new Dictionary<string, string>
                {
                    { displayName, relativePath }
                });
            }
        }
        
        return navItems;
    }
    
    static object? ProcessDirectory(string dirPath, string docsRoot)
    {
        var dirName = new DirectoryInfo(dirPath).Name;
        var displayName = FormatDisplayName(dirName);
        var relativePath = Path.GetRelativePath(docsRoot, dirPath).Replace("\\", "/");
        
        // Get all markdown files and subdirectories
        var mdFiles = Directory.GetFiles(dirPath, "*.md", SearchOption.TopDirectoryOnly)
            .OrderBy(f => Path.GetFileName(f))
            .ToList();
        
        var subDirs = Directory.GetDirectories(dirPath)
            .OrderBy(d => Path.GetFileName(d))
            .ToList();
        
        var children = new List<object>();
        
        // Process files in this directory
        foreach (var file in mdFiles)
        {
            var fileName = Path.GetFileName(file);
            var fileDisplayName = FormatDisplayName(Path.GetFileNameWithoutExtension(fileName));
            var fileRelativePath = Path.GetRelativePath(docsRoot, file).Replace("\\", "/");
            
            children.Add(new Dictionary<string, string>
            {
                { fileDisplayName, fileRelativePath }
            });
        }
        
        // Process subdirectories
        foreach (var subDir in subDirs)
        {
            var subDirItem = ProcessDirectory(subDir, docsRoot);
            if (subDirItem != null)
            {
                children.Add(subDirItem);
            }
        }
        
        if (children.Count == 0)
        {
            return null; // Skip empty directories
        }
        
        return new Dictionary<string, object>
        {
            { displayName, children }
        };
    }
    
    static string FormatDisplayName(string name)
    {
        // Remove leading numbers (e.g., "01_", "10_", "001_")
        var result = Regex.Replace(name, @"^\d+_?", "");
        
        // Replace underscores with spaces
        result = result.Replace("_", " ");
        
        return result;
    }
    
    static void UpdateMkDocsYaml(string mkdocsPath, List<object> navItems)
    {
        // Read the existing mkdocs.yml content
        var yamlContent = File.ReadAllText(mkdocsPath);
        
        // Deserialize the YAML
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        var mkdocsConfig = deserializer.Deserialize<Dictionary<object, object>>(yamlContent);
        
        // Add or update the nav section
        mkdocsConfig["nav"] = navItems;
        
        // Serialize back to YAML
        var serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        var updatedYaml = serializer.Serialize(mkdocsConfig);
        
        // Write back to file
        File.WriteAllText(mkdocsPath, updatedYaml);
        
        Console.WriteLine($"Updated mkdocs.yml with {navItems.Count} top-level navigation items");
    }
}
