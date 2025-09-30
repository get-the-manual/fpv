using Renamer;

// Dictionary for rename mappings: key = old path/name, value = new name
var renameMappings = PathHelper.GetFoldersForRenaming();
//    new Dictionary<string, string>
//{
//     { @"docs\00_Drones\30_Камеры", "30_Camera" },
//     { @"docs\00_Drones\35_Моторы", "35_Motor" },
//     { @"docs\00_Drones\40_Пропеллеры", "40_Prop" },
//     { @"docs\00_Drones\45_Рамы", "45_Frame" },
//     { @"docs\00_Drones\30_Buzzer_(пищалка).md", "30_Buzzer.md" },
//};

var dryRun = args.Length > 0 && args[0] == "--dry-run1";
var repoRoot = PathHelper.FindRepositoryRoot();
var docsPath = Path.Combine(repoRoot, "docs");
var mkdocsPath = Path.Combine(repoRoot, "mkdocs.yml");

if (renameMappings.Count == 0)
{
    Console.WriteLine("No rename mappings defined in the dictionary.");
    Console.WriteLine("Add mappings to the renameMappings dictionary in Program.cs");
    return 1;
}

var renameCount = 0;

// Iterate over all mappings and perform renames
foreach (var mapping in renameMappings)
{
    var inputPath = mapping.Key;
    var newName = mapping.Value;

    Console.WriteLine();
    Console.WriteLine($"Processing: {inputPath} -> {newName}");

    // Resolve the input path (can be relative or absolute)
    var absolutePath = Path.IsPathRooted(inputPath) 
        ? inputPath 
        : Path.GetFullPath(Path.Combine(repoRoot, inputPath));

    // Validate that the path exists
    if (!File.Exists(absolutePath) && !Directory.Exists(absolutePath))
    {
        Console.WriteLine($"Warning: Path does not exist: {absolutePath}");
        Console.WriteLine("Skipping this mapping.");
        continue;
    }

    // Validate that the path is inside docs folder
    if (!absolutePath.StartsWith(docsPath, StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"Warning: Path must be inside the docs folder: {docsPath}");
        Console.WriteLine("Skipping this mapping.");
        continue;
    }

    // Get the last part of the path (file or folder name)
    var originalName = Path.GetFileName(absolutePath);
    Console.WriteLine($"  Original name: {originalName}");
    Console.WriteLine($"  New name: {newName}");

    // Check if the name is actually different
    if (originalName == newName)
    {
        Console.WriteLine("  No changes needed - the names are the same.");
        continue;
    }

    // Construct the new path
    var parentDir = Path.GetDirectoryName(absolutePath)!;
    var newPath = Path.Combine(parentDir, newName);

    // Check if target already exists
    if (File.Exists(newPath) || Directory.Exists(newPath))
    {
        Console.WriteLine($"  Warning: Target path already exists: {newPath}");
        Console.WriteLine("  Skipping this mapping.");
        continue;
    }

    // Calculate relative paths for reference updates
    var oldRelativePath = Path.GetRelativePath(repoRoot, absolutePath).Replace("\\", "/");
    var newRelativePath = Path.GetRelativePath(repoRoot, newPath).Replace("\\", "/");

    if (dryRun)
    {
        Console.WriteLine($"  [DRY RUN] Would rename: {absolutePath} -> {newPath}");
        Console.WriteLine($"  [DRY RUN] Would update references from '{oldRelativePath}' to '{newRelativePath}'");
    }
    else
    {
        // Rename the file or folder
        Console.WriteLine($"  Renaming: {absolutePath} -> {newPath}");
        if (File.Exists(absolutePath))
        {
            File.Move(absolutePath, newPath);
        }
        else
        {
            Directory.Move(absolutePath, newPath);
        }

        // Update references
        Console.WriteLine($"  Updating references from '{oldRelativePath}' to '{newRelativePath}'...");

        // Update mkdocs.yml
        if (File.Exists(mkdocsPath))
        {
            ReferenceUpdater.UpdateMkdocsYml(mkdocsPath, originalName, newName);
            Console.WriteLine("  Updated mkdocs.yml");
        }

        // Update all .md files in docs folder
        var updatedFiles = ReferenceUpdater.UpdateMarkdownFiles(docsPath, originalName, newName);
        Console.WriteLine($"  Updated {updatedFiles} markdown file(s)");

        Console.WriteLine("  Rename completed successfully!");
    }

    renameCount++;
}

Console.WriteLine();
if (dryRun)
{
    Console.WriteLine($"[DRY RUN] Would process {renameCount} rename(s). No actual changes made.");
}
else
{
    Console.WriteLine($"Processed {renameCount} rename(s).");
}

return 0;
