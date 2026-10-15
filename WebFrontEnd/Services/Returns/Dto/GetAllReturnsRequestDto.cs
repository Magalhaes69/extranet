using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Services.Returns.Dto
{
    public class GetAllReturnsRequestDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? CardCode { get; set; }
        public List<string>? Cancelled { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
