using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Shared.Dtos
{
    public abstract class SalesRequest
    {
     public string? CardCode { get; set; }
    public List<string>? DocumentStatus { get; set; }
    public List<string>? Cancelled { get; set; }

    public List<DynamicFilter>? DynamicFilters { get; set; }
}
}
