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
}
