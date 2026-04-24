
namespace Application.UseCases.SQLQueries
{
    public sealed record SapQueryResponse
    {
        public int Series { get; set; }
        public string? SeriesName { get; set; } 
    }
}
