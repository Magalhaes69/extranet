using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Shared.Dtos
{
    public class TaxIVARequest
    {
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
