namespace WebFrontEnd.Shared.Dtos
{
    public class SidebarMenuItem
    {
        public string Key { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        /// <summary>Chave de tradução do título da secção (cabeçalho) onde o item começa.</summary>
        public string? Section { get; set; }
        public List<SidebarMenuItem> Children { get; set; } = new();
    }
}
