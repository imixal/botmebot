using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.AntiSpamModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class AntiSpamSettingsRepository : Repository<AntiSpamSetting>
    {
        public AntiSpamSettingsRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<List<AntiSpamSetting>> GetAllAsync()
        {
            return await GetAllQuery()
                .Include(s => s.Chat)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}