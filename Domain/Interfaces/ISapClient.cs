using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISapClient
    {
        Task<bool> LoginAsync(CancellationToken cancellationToken = default);
        Task LogoutAsync();
        Task<T> AddAsync<T>(string objectType, object obj, CancellationToken cancellationToken);
        Task<T?> GetAsync<T>(string objectType, string? odataFilter = null, CancellationToken cancellationToken = default) where T : class;
        Task<List<T>> GetAllAsync<T>(string objectType, string? odataFilter = null, CancellationToken cancellationToken = default);
    
}
}
