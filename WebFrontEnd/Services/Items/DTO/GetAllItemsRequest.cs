using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Services.Items.DTO
{
    public class GetAllItemsRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Valid { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
