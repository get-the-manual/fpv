using System.Text;

namespace BuildNavigation;

public class NavYamlGenerator
{
    public static string Generate(NavItem root)
    {
        var sb = new StringBuilder();
        sb.AppendLine("nav:");

        foreach (var child in root.Children)
            AppendNavItem(sb, child, 1);

        return sb.ToString();
    }

    private static void AppendNavItem(StringBuilder sb, NavItem item, int level)
    {
        var indent = new string(' ', level * 2);

        if (item.IsDirectory)
        {
            sb.AppendLine($"{indent}- {item.Name}:");
            foreach (var child in item.Children)
                AppendNavItem(sb, child, level + 1);
        }
        else
            sb.AppendLine($"{indent}- {item.Name}: {item.Path}");
    }
}