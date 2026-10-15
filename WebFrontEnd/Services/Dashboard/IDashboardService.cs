using WebFrontEnd.Services.Dashboard.Dtos;
using WebFrontEnd.Services.Orders.Models;

namespace WebFrontEnd.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<GetCountDto> GetCountOrderAsync(string CardCode, string Status);
        Task<DashboardMetricDto?> GetMetricAsync(string key, CancellationToken ct = default);
    }
}
