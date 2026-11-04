namespace WebFrontEnd.Shared.Dtos
{
    public class SidebarMenuItem
    {
        public string Key { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        /// <summary>Only shown to Manager/Administrator.</summary>
        public bool AdminOnly { get; set; }
        public List<SidebarMenuItem> Children { get; set; } = new();
    }
}
