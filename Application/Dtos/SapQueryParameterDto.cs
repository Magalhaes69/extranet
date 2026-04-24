
namespace Application.Dtos
{
    public record SapQueryParameterdTO
    {
        public string Name { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
