namespace BuildNavigation;

public class MkdocsYmlProcessor
{
    public static void UpdateNavSection(string mkdocsPath, string navYaml)
    {
        var lines = File.ReadAllLines(mkdocsPath).ToList();

        // Find if nav section already exists
        var navStartIndex = -1;
        for (var i = 0; i < lines.Count; i++)
        {
            if (lines[i].Trim().StartsWith("nav:"))
            {
                navStartIndex = i;
                break;
            }
        }

        // Remove existing nav section if it exists
        if (navStartIndex >= 0)
        {
            var navEndIndex = navStartIndex + 1;
            while (navEndIndex < lines.Count &&
                   (lines[navEndIndex].StartsWith("  ") || lines[navEndIndex].StartsWith("\t") || string.IsNullOrWhiteSpace(lines[navEndIndex])))
                navEndIndex++;
            lines.RemoveRange(navStartIndex, navEndIndex - navStartIndex);

            // Remove trailing empty lines before nav
            while (navStartIndex > 0 && string.IsNullOrWhiteSpace(lines[navStartIndex - 1]))
            {
                lines.RemoveAt(navStartIndex - 1);
                navStartIndex--;
            }
        }

        // Add new nav section at the end
        if (lines.Count > 0 && !string.IsNullOrWhiteSpace(lines[^1]))
            lines.Add("");
        lines.AddRange(navYaml.TrimEnd().Split(Environment.NewLine));

        File.WriteAllText(mkdocsPath, string.Join(Environment.NewLine, lines));
    }
}