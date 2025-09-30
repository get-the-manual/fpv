namespace BuildNavigation;

public class NavItem
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public bool IsDirectory { get; set; }
    public List<NavItem> Children { get; set; } = new List<NavItem>();
}