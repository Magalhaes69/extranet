
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISapClient
    {
        Task<bool> LoginAsync(CancellationToken cancellationToken = default);
        Task LogoutAsync();
        /// <summary>
        /// Synchronously pushes a business object to the SAP DI-API/Server Layer.
        /// </summary>
        /// <typeparam name="T">The specific SAP Business Object type(e.g, Orders, Invoices, etc)</typeparam>
        /// <param name="objectType">The SAP business object type identifier.</param>
        /// <param name="obj">The data entity containing the fields to be populated in SAP.</param>
        /// <param name="cancellationToken">A token to abort the SAP integration process.</param>
        /// <returns>A task representing the operation, returning the entry key of the newly created object.</returns>
        /// <exception cref="SapException">Thrown when the SAP SDK returns a non-zero error code.</exception>
        Task<T> AddAsync<T>(string objectType, object obj, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves a single business object from the SAP Service Layer using OData filtering.
        /// </summary>
        /// <typeparam name="T">The data transfer object (DTO) class representing the SAP entity.</typeparam>
        /// <param name="objectType">The SAP Service Layer resource name (e.g., "BusinessPartners", "Orders").</param>
        /// <param name="odataFilter">An optional OData $filter string. If provided, the query is constrained to the first match ($top=1).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the network operation.</param>
        /// <returns>The first matching entity of type <typeparamref name="T"/>, or <see langword="null"/> if no match is found.</returns>
        /// <exception cref="HttpRequestException">Thrown if the SAP Service Layer returns a non-success status code.</exception>
        /// <exception cref="TaskCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<T?> GetAsync<T>(string objectType, string? odataFilter = null, CancellationToken cancellationToken = default) where T : class;
        Task<List<T>> GetAllAsync<T>(
            string objectType,
            string?
            odataFilter = null,
            CancellationToken
            cancellationToken = default);

        /// <summary>
        /// Metodo para retornar a lista de um objecto
        /// </summary>
        /// <typeparam name="T">Objecto definido pelo utilizador</typeparam>
        /// <param name="objectType"></param>
        /// <param name="filter"></param>
        /// <param name="queryParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
            string? field,
            CancellationToken cancellationToken);
        Task<T?> ExecuteQueryObjAsync<T>(
            string queryName,
            Dictionary<string, object>? parameters,
            CancellationToken cancellationToken) where T : class;

       
        Task<T?> TryGetAsync<T>(string path, CancellationToken cancellationToken) where T : class;
        Task PatchAsync(string path, object obj, CancellationToken cancellationToken);

        Task<string> GetLocalCurrencyAsync(CancellationToken cancellationToken);

        Task<int> CountAsync(string entity, string? filter, CancellationToken ct);
        Task<decimal[]> SumAsync(string entity, IReadOnlyList<string> fields, string? filter, CancellationToken ct);

        Task<List<T>> ExecuteQueryListAsync<T>(
          string queryName,
          Dictionary<string, object>? parameters,
          CancellationToken cancellationToken) where T : class;
    }
}
