using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.CustomMessageModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class CustomMessageRepository : Repository<CustomMessage>
    {
        public CustomMessageRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<List<CustomMessage>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(cm => cm.Chat)
                .ToListAsync();
        }

        public override async Task<CustomMessage> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(cm => cm.Chat)
                .FirstOrDefaultAsync(cm=> cm.Id == id);
        }

        public override async Task<CustomMessage> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(cm => cm.Chat)
                .FirstOrDefaultAsync(cm => cm.Id == id);
        }
    }
}