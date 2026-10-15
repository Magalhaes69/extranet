namespace WebFrontEnd.Services.Dashboard.Dtos
{
    public sealed class DashboardMetricDto
    {
        public string Key { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
    }
}
