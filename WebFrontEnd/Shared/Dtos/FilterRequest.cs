using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Shared.Dtos
{
    public class FilterRequest
    {
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
