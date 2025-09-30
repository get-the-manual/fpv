using NameTransformer;

if (args.Length == 0)
{
    Console.WriteLine("Usage: NameTransformer <path>");
    Console.WriteLine("  <path> - Path to a file or folder inside the docs directory");
    return 1;
}

var inputPath = args[0];
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

// Transliterate the name
var transliteratedName = CyrillicTransliterator.Transliterate(originalName);
Console.WriteLine($"Transliterated name: {transliteratedName}");

// Check if the name changed
if (originalName == transliteratedName)
{
    Console.WriteLine("No changes needed - the name does not contain Cyrillic characters.");
    return 0;
}

// Construct the new path
var parentDir = Path.GetDirectoryName(absolutePath)!;
var newPath = Path.Combine(parentDir, transliteratedName);

// Check if target already exists
if (File.Exists(newPath) || Directory.Exists(newPath))
{
    Console.WriteLine($"Error: Target path already exists: {newPath}");
    return 1;
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
var oldRelativePath = Path.GetRelativePath(repoRoot, absolutePath).Replace("\\", "/");
var newRelativePath = Path.GetRelativePath(repoRoot, newPath).Replace("\\", "/");

Console.WriteLine($"Updating references from '{oldRelativePath}' to '{newRelativePath}'...");

// Update mkdocs.yml
if (File.Exists(mkdocsPath))
{
    ReferenceUpdater.UpdateMkdocsYml(mkdocsPath, originalName, transliteratedName);
    Console.WriteLine("Updated mkdocs.yml");
}

// Update all .md files in docs folder
var updatedFiles = ReferenceUpdater.UpdateMarkdownFiles(docsPath, originalName, transliteratedName);
Console.WriteLine($"Updated {updatedFiles} markdown file(s)");

Console.WriteLine("Transformation completed successfully!");
return 0;
