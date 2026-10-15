
namespace Application.UseCases.Dashboard
{
    public sealed record DashboardMetricDefinition(
    string Entity,
    Func<string, string> BuildFilter,             // cardCode -> OData filter
    IReadOnlyList<(string Field, int Sign)> Amount,
    bool IncludeChecks = false);
}
