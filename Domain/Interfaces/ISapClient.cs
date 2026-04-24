
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISapClient
    {
        Task<bool> LoginAsync(CancellationToken cancellationToken = default);
        Task LogoutAsync();
        Task<T> AddAsync<T>(string objectType, object obj, CancellationToken cancellationToken);
        Task<T?> GetAsync<T>(string objectType, string? odataFilter = null, CancellationToken cancellationToken = default) where T : class;
        Task<List<T>> GetAllAsync<T>(
            string objectType,
            string?
            odataFilter = null,
            CancellationToken
            cancellationToken = default);


        Task<List<T>> GetAllB1Async<T>(
            string objectType,
            string? filter,
            Dictionary<string, string>? queryParams,
            CancellationToken cancellationToken);
        Task<T> GetB1Async<T>(
                string objectType,
                string? filter,
                Dictionary<string, string>? queryParams,
                CancellationToken cancellationToken);

        Task<GetAttachments2> GetAttachments2Async(string url, CancellationToken cancellationToken);

        Task<List<T>> GetPagedB1Async<T>(
            string objectType,
            int page,
            int pageSize = 20,
            string? filter = null,
            Dictionary<string, string>? queryParams = null,
            CancellationToken cancellationToken = default);

        Task<(List<T> Data, int TotalCount)> GetPagedBL1Async<T>(
           string objectType,
           int page,
           int pageSize = 20,
           string? filter = null,
           Dictionary<string, string>? queryParams = null,
           CancellationToken cancellationToken = default);

        Task GetBackgroundB1Async<T>(
            string objectType,
            Action<List<T>> onPageLoaded,
            string? filter = null,
            Dictionary<string, string>? queryParams = null,
            CancellationToken cancellationToken = default);

        Task<T> GetTotalsAsync<T>(
            string objectType,
            string filter,
            CancellationToken cancellationToken);

        Task<List<T>> ExecuteQueryAsync<T>(
            string objectType,
            string? objectCode,
            CancellationToken cancellationToken);
    }
}
