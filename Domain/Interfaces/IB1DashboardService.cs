using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IB1DashboardService
    {
        Task<List<T>> GetListAsync<T>(string endpoint, string filter, CancellationToken cancellationToken);
    }
}
