using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace BuildNavigation;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Building navigation for mkdocs.yml...");

        // Get the repository root directory (two levels up from SiteUtils/BuildNavigation)
        var currentDir = Directory.GetCurrentDirectory();
        var repoRoot = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
        var docsPath = Path.Combine(repoRoot, "docs");
        var mkdocsYmlPath = Path.Combine(repoRoot, "mkdocs.yml");

        if (!Directory.Exists(docsPath))
        {
            Console.WriteLine($"Error: docs folder not found at {docsPath}");
            return;
        }

        if (!File.Exists(mkdocsYmlPath))
        {
            Console.WriteLine($"Error: mkdocs.yml not found at {mkdocsYmlPath}");
            return;
        }

        // Build navigation structure
        var nav = BuildNavigation(docsPath);

        // Update mkdocs.yml
        UpdateMkdocsYml(mkdocsYmlPath, nav);

        Console.WriteLine("Navigation successfully built and added to mkdocs.yml");
    }

    static List<object> BuildNavigation(string docsPath)
    {
        var nav = new List<object>();

        // Get all markdown files
        var mdFiles = Directory.GetFiles(docsPath, "*.md", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(docsPath, f))
            .OrderBy(f => f)
            .ToList();

        // Group by top-level directory
        var groups = mdFiles
            .GroupBy(f => f.Contains(Path.DirectorySeparatorChar) ? f.Split(Path.DirectorySeparatorChar)[0] : "")
            .OrderBy(g => g.Key);

        foreach (var group in groups)
        {
            if (string.IsNullOrEmpty(group.Key))
            {
                // Root level files
                foreach (var file in group)
                {
                    var title = FormatTitle(Path.GetFileNameWithoutExtension(file));
                    nav.Add(new Dictionary<string, string> { { title, file.Replace('\\', '/') } });
                }
            }
            else
            {
                // Subdirectories
                var dirTitle = FormatTitle(group.Key);
                var subNav = BuildSubNavigation(docsPath, group.Key);
                nav.Add(new Dictionary<string, object> { { dirTitle, subNav } });
            }
        }

        return nav;
    }

    static List<object> BuildSubNavigation(string docsPath, string relativePath)
    {
        var subNav = new List<object>();
        var fullPath = Path.Combine(docsPath, relativePath);

        // Get files in current directory
        var files = Directory.GetFiles(fullPath, "*.md")
            .Select(f => Path.GetRelativePath(docsPath, f))
            .OrderBy(f => f)
            .ToList();

        foreach (var file in files)
        {
            var title = FormatTitle(Path.GetFileNameWithoutExtension(file));
            subNav.Add(new Dictionary<string, string> { { title, file.Replace('\\', '/') } });
        }

        // Get subdirectories
        var dirs = Directory.GetDirectories(fullPath)
            .Select(d => Path.GetRelativePath(docsPath, d))
            .OrderBy(d => d)
            .ToList();

        foreach (var dir in dirs)
        {
            var dirName = Path.GetFileName(dir);
            var dirTitle = FormatTitle(dirName);
            var subSubNav = BuildSubNavigation(docsPath, dir);
            if (subSubNav.Count > 0)
            {
                subNav.Add(new Dictionary<string, object> { { dirTitle, subSubNav } });
            }
        }

        return subNav;
    }

    static string FormatTitle(string name)
    {
        // Remove leading numbers (e.g., "00_", "10_", etc.)
        var withoutNumbers = Regex.Replace(name, @"^\d+_", "");
        
        // Replace underscores with spaces
        var withSpaces = withoutNumbers.Replace('_', ' ');
        
        return withSpaces;
    }

    static void UpdateMkdocsYml(string mkdocsYmlPath, List<object> nav)
    {
        // Read the existing content
        var content = File.ReadAllText(mkdocsYmlPath);

        // Parse YAML
        var input = new StringReader(content);
        var yaml = new YamlStream();
        yaml.Load(input);

        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

        // Remove existing nav if present
        if (mapping.Children.ContainsKey(new YamlScalarNode("nav")))
        {
            mapping.Children.Remove(new YamlScalarNode("nav"));
        }

        // Build new nav node
        var navNode = BuildYamlNode(nav);
        mapping.Add("nav", navNode);

        // Write back to file
        using var writer = new StringWriter();
        yaml.Save(writer, false);
        var newContent = writer.ToString();

        File.WriteAllText(mkdocsYmlPath, newContent);
    }

    static YamlNode BuildYamlNode(object obj)
    {
        if (obj is List<object> list)
        {
            var sequence = new YamlSequenceNode();
            foreach (var item in list)
            {
                sequence.Add(BuildYamlNode(item));
            }
            return sequence;
        }
        else if (obj is Dictionary<string, object> dictObj)
        {
            var mapping = new YamlMappingNode();
            foreach (var kvp in dictObj)
            {
                mapping.Add(kvp.Key, BuildYamlNode(kvp.Value));
            }
            return mapping;
        }
        else if (obj is Dictionary<string, string> dictStr)
        {
            var mapping = new YamlMappingNode();
            foreach (var kvp in dictStr)
            {
                mapping.Add(kvp.Key, kvp.Value);
            }
            return mapping;
        }
        else
        {
            return new YamlScalarNode(obj?.ToString() ?? "");
        }
    }
}
