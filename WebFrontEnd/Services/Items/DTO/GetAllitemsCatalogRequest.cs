namespace WebFrontEnd.Services.Items.DTO
{
    public class GetAllitemsCatalogRequest
    {
        public string Valid { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TPriceBusiness { get; set; } = 1;
        public List<int> GroupNumbes { get; set; } = new();
    }
}
