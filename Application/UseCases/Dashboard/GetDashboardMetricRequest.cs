using Domain.Helps.Enum;
using MediatR;

namespace Application.UseCases.Dashboard
{
    public sealed record GetDashboardMetricRequest(string Key, string CardCode) : IRequest<DashboardMetricResponse>;

//    public sealed record GetDashboardMetricRequest(string Key, string CardCode, MetricPart Part)
//    : IRequest<DashboardMetricResponse>;
}
