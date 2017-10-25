using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.CommandModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class ChatCommandRepository : Repository<ChatCommand>
    {
        public ChatCommandRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<List<ChatCommand>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Chat)
                .Include(cc => cc.Command)
                .ToListAsync();
        }

        public override async Task<ChatCommand> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(cc => cc.Chat)
                .Include(cc => cc.Command)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public override async Task<ChatCommand> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Chat)
                .Include(cc => cc.Command)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
    }
}