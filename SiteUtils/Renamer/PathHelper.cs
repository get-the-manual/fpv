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

        // Scan for markdown files with Cyrillic characters
        var mdFiles = Directory.GetFiles(docsPath, "*.md", SearchOption.AllDirectories);

        foreach (var file in mdFiles)
        {
            var fileName = Path.GetFileName(file);

            // Skip hidden files
            if (fileName.StartsWith("."))
                continue;

            // Check if filename contains Cyrillic characters
            if (ContainsCyrillic(fileName))
            {
                var relativePath = Path.GetRelativePath(repoRoot, file);
                var transliteratedName = CyrillicTransliterator.Transliterate(fileName);
                renameMappings[relativePath] = transliteratedName;
            }
        }

        return renameMappings;
    }

    public static Dictionary<string, string> GetFoldersForRenaming()
    {
        var repoRoot = FindRepositoryRoot();
        var docsPath = Path.Combine(repoRoot, "docs");
        var renameMappings = new Dictionary<string, string>();

        if (!Directory.Exists(docsPath))
        {
            return renameMappings;
        }

        // Scan for folders with Cyrillic characters
        var directories = Directory.GetDirectories(docsPath, "*", SearchOption.AllDirectories);

        foreach (var directory in directories)
        {
            var folderName = Path.GetFileName(directory);

            // Skip hidden folders
            if (folderName.StartsWith("."))
                continue;

            // Check if folder name contains Cyrillic characters
            if (ContainsCyrillic(folderName))
            {
                var relativePath = Path.GetRelativePath(repoRoot, directory);
                var transliteratedName = CyrillicTransliterator.Transliterate(folderName);
                renameMappings[relativePath] = transliteratedName;
                return renameMappings;
            }
        }

        return renameMappings;
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
