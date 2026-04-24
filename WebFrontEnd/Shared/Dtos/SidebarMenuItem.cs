namespace WebFrontEnd.Shared.Dtos
{
    public class SidebarMenuItem
    {
        public string Key { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public List<SidebarMenuItem> Children { get; set; } = new();
    }
}
