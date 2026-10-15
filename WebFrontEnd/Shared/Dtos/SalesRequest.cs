using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Shared.Dtos
{
    public abstract class SalesRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string? CardCode { get; set; }
        public List<string>? DocumentStatus { get; set; }
        public List<string>? Cancelled { get; set; }

        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
