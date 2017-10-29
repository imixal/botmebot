using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.AntiSpamModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class WhiteLinkRepository : Repository<WhiteLink>
    {
        public WhiteLinkRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<List<WhiteLink>> GetAllAsync()
        {
            return await GetAllQuery()
                .Include(wl => wl.Chat)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}