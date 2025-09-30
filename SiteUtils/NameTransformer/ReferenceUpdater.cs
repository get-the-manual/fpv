using System.Text;
using System.Text.RegularExpressions;

namespace NameTransformer;

public static partial class ReferenceUpdater
{
    public static void UpdateMkdocsYml(string mkdocsPath, string oldName, string newName)
    {
        var content = File.ReadAllText(mkdocsPath);
        
        // Replace references in the nav section
        // This will replace the name in paths like: "00_Дроны(Квадрокоптеры)/file.md"
        var updated = content.Replace(oldName, newName);
        
        if (updated != content)
        {
            File.WriteAllText(mkdocsPath, updated);
        }
    }

    public static int UpdateMarkdownFiles(string docsPath, string oldName, string newName)
    {
        var updatedCount = 0;
        var mdFiles = Directory.GetFiles(docsPath, "*.md", SearchOption.AllDirectories);

        foreach (var file in mdFiles)
        {
            var content = File.ReadAllText(file);
            var updated = content.Replace(oldName, newName);

            if (updated != content)
            {
                File.WriteAllText(file, updated);
                updatedCount++;
            }
        }

        return updatedCount;
    }
}
