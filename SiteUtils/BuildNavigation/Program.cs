using BuildNavigation;

var repoRoot = FindRepositoryRoot();
var docsPath = Path.Combine(repoRoot, "docs");
var mkdocsPath = Path.Combine(repoRoot, "mkdocs.yml");

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
var navStructure = NavigationStructureBuilder.BuildFromFolder(docsPath);

Console.WriteLine("Generating navigation YAML...");
var navYaml = NavYamlGenerator.Generate(navStructure);

Console.WriteLine("Updating mkdocs.yml...");
MkdocsYmlProcessor.UpdateNavSection(mkdocsPath, navYaml);

Console.WriteLine("Navigation section generated successfully!");

static string FindRepositoryRoot()
{
    var currentDir = Directory.GetCurrentDirectory();

    while (currentDir != null)
    {
        if (Directory.Exists(Path.Combine(currentDir, ".git")))
            return currentDir;

        currentDir = Directory.GetParent(currentDir)?.FullName;
    }

    throw new("Could not find repository root (no .git folder found). Please run this tool from within a Git repository.");
}
