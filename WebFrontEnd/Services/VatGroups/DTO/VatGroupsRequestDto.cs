using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Services.VatGroups.DTO
{
    public class VatGroupsRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Inactive { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
