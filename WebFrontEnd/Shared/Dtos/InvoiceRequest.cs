using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Shared.Dtos
{
    public class InvoiceRequest
    {
        public string? CardCode { get; set; }
        public List<string>? DocumentStatuses { get; set; }
        public List<string>? Cancelled { get; set; }

        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
