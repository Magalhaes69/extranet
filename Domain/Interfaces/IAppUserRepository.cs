using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IAppUserRepository : IBaseUserRepository
    {
        Task<AppUser> GetByEmail(string email, CancellationToken cancellationToken);
    }
}
