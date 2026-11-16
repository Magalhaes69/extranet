using WebFrontEnd.Services.Dashboard.Dtos;

namespace WebFrontEnd.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<ClientDashboardDto?> GetClientDashboardAsync(CancellationToken ct = default);
        Task<DashboardMetricDto?> GetMetricAsync(string key, CancellationToken ct = default);
    }
}
