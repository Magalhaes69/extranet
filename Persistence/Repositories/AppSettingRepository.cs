

using Domain.Interfaces;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class AppSettingRepository : BaseRepository<AppSetting>, IAppSettingRepository
    {
        public AppSettingRepository(AppDbContext context) : base(context) { } 
    }
}
