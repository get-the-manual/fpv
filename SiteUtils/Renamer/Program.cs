using Renamer;

// Dictionary for rename mappings: key = old name, value = new name
var renameMappings = new Dictionary<string, string>
{
    // Example mappings - add your rename patterns here
    // { "OldName.md", "NewName.md" },
    // { "OldFolder", "NewFolder" },
};

if (args.Length == 0)
{
    Console.WriteLine("Usage: Renamer <path> [--dry-run]");
    Console.WriteLine("  <path>     - Path to a file or folder inside the docs directory");
    Console.WriteLine("  --dry-run  - Preview changes without applying them");
    Console.WriteLine();
    Console.WriteLine("The tool uses a dictionary of rename mappings defined in Program.cs");
    return 1;
}

var inputPath = args[0];
var dryRun = args.Length > 1 && args[1] == "--dry-run";
var repoRoot = PathHelper.FindRepositoryRoot();
var docsPath = Path.Combine(repoRoot, "docs");
var mkdocsPath = Path.Combine(repoRoot, "mkdocs.yml");

// Validate that the path exists
if (!File.Exists(inputPath) && !Directory.Exists(inputPath))
{
    Console.WriteLine($"Error: Path does not exist: {inputPath}");
    return 1;
}

// Get absolute path
var absolutePath = Path.GetFullPath(inputPath);

// Validate that the path is inside docs folder
if (!absolutePath.StartsWith(docsPath, StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine($"Error: Path must be inside the docs folder: {docsPath}");
    return 1;
}

// Get the last part of the path (file or folder name)
var originalName = Path.GetFileName(absolutePath);
Console.WriteLine($"Original name: {originalName}");

// Check if there's a mapping for this name
if (!renameMappings.TryGetValue(originalName, out var newName))
{
    Console.WriteLine($"No rename mapping found for: {originalName}");
    Console.WriteLine("Add a mapping to the renameMappings dictionary in Program.cs");
    return 1;
}

Console.WriteLine($"New name: {newName}");

// Check if the name is actually different
if (originalName == newName)
{
    Console.WriteLine("No changes needed - the names are the same.");
    return 0;
}

// Construct the new path
var parentDir = Path.GetDirectoryName(absolutePath)!;
var newPath = Path.Combine(parentDir, newName);

// Check if target already exists
if (File.Exists(newPath) || Directory.Exists(newPath))
{
    Console.WriteLine($"Error: Target path already exists: {newPath}");
    return 1;
}

// Calculate relative paths for reference updates
var oldRelativePath = Path.GetRelativePath(repoRoot, absolutePath).Replace("\\", "/");
var newRelativePath = Path.GetRelativePath(repoRoot, newPath).Replace("\\", "/");

if (dryRun)
{
    Console.WriteLine($"[DRY RUN] Would rename: {absolutePath} -> {newPath}");
    Console.WriteLine($"[DRY RUN] Would update references from '{oldRelativePath}' to '{newRelativePath}'");
    Console.WriteLine("[DRY RUN] No actual changes made.");
    return 0;
}

// Rename the file or folder
Console.WriteLine($"Renaming: {absolutePath} -> {newPath}");
if (File.Exists(absolutePath))
{
    File.Move(absolutePath, newPath);
}
else
{
    Directory.Move(absolutePath, newPath);
}

// Update references

Console.WriteLine($"Updating references from '{oldRelativePath}' to '{newRelativePath}'...");

// Update mkdocs.yml
if (File.Exists(mkdocsPath))
{
    ReferenceUpdater.UpdateMkdocsYml(mkdocsPath, originalName, newName);
    Console.WriteLine("Updated mkdocs.yml");
}

// Update all .md files in docs folder
var updatedFiles = ReferenceUpdater.UpdateMarkdownFiles(docsPath, originalName, newName);
Console.WriteLine($"Updated {updatedFiles} markdown file(s)");

Console.WriteLine("Rename completed successfully!");
return 0;
