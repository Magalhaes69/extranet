

namespace Application.Dtos
{
    public record TotalResult
    {
        public decimal Total { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
