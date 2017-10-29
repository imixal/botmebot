using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.AntiSpamModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class SuspiciousUserRepository : Repository<SuspiciousUser>
    {
        public SuspiciousUserRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<List<SuspiciousUser>> GetAllAsync()
        {
            return await GetAllQuery()
                .Include(su => su.Chat)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<SuspiciousUser> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(su => su.Chat)
                .FirstOrDefaultAsync(su => su.Id == id);
        }

        public override async Task<SuspiciousUser> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .Include(su => su.Chat)
                .AsNoTracking()
                .FirstOrDefaultAsync(su => su.Id == id);
        }
    }
}