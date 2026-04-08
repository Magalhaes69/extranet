
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


        Task<GetAttachments2> GetAttachments2Async(string url, CancellationToken cancellationToken);
    }
}
