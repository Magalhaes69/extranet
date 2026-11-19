using WebFrontEnd.Services.SQLQueries.Dto;

namespace WebFrontEnd.Services.SQLQueries
{
    public interface ISQLQueriesService
    {
        Task<List<GetSeriesDynamicResult>> ExecuteQueryAsync(
           GetSeriesDynamicRequest request);

        Task<ContactEmployeesResponse> ExecuteQueryContactAsync(
        ContactEmployeesRequest request);
    }
}
