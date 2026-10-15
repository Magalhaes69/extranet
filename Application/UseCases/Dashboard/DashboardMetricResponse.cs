
namespace Application.UseCases.Dashboard
{
    public sealed record DashboardMetricResponse
    {
        public string Key { get; init; } = string.Empty;
        public int? Count { get; init; }
        public decimal? Amount { get; init; }
        public string? Currency { get; init; }
        public DateTime LoadedAtUtc { get; init; }
        public bool IsStale { get; init; }
    }
}
