namespace Renamer;

public static class PathHelper
{
    public static string FindRepositoryRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();

        while (currentDir != null)
        {
            if (Directory.Exists(Path.Combine(currentDir, ".git")))
                return currentDir;

            currentDir = Directory.GetParent(currentDir)?.FullName;
        }

        throw new InvalidOperationException("Could not find repository root (no .git folder found). Please run this tool from within a Git repository.");
    }

    public static Dictionary<string, string> GetFilesForRenaming()
    {
        var repoRoot = FindRepositoryRoot();
        var docsPath = Path.Combine(repoRoot, "docs");
        var renameMappings = new Dictionary<string, string>();

        if (!Directory.Exists(docsPath))
        {
            return renameMappings;
        }

        // Scan for files and directories with Cyrillic characters
        ScanDirectory(docsPath, repoRoot, renameMappings);

        return renameMappings;
    }

    private static void ScanDirectory(string path, string repoRoot, Dictionary<string, string> mappings)
    {
        // Get all files and directories in the current path
        var entries = Directory.GetFileSystemEntries(path);

        foreach (var entry in entries)
        {
            var name = Path.GetFileName(entry);

            // Skip hidden files/folders
            if (name.StartsWith("."))
                continue;

            // Check if name contains Cyrillic characters
            if (ContainsCyrillic(name))
            {
                var relativePath = Path.GetRelativePath(repoRoot, entry);
                // The value would be the transliterated name, but for now we'll leave it empty
                // as the actual transliteration logic would be in a separate method
                mappings[relativePath] = string.Empty;
            }

            // Recursively scan subdirectories
            if (Directory.Exists(entry))
            {
                ScanDirectory(entry, repoRoot, mappings);
            }
        }
    }

    private static bool ContainsCyrillic(string text)
    {
        foreach (var c in text)
        {
            if ((c >= 0x0400 && c <= 0x04FF) || // Cyrillic block
                (c >= 0x0500 && c <= 0x052F))   // Cyrillic Supplement
            {
                return true;
            }
        }
        return false;
    }
}
