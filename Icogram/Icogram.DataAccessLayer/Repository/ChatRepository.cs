using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ChatModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class ChatRepository : Repository<Chat>
    {
        public ChatRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<List<Chat>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Commands)
                .Include(cc => cc.Company)
                .ToListAsync();
        }

        public override async Task<Chat> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(cc => cc.Company)
                .Include(cc => cc.Commands)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public override async Task<Chat> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Company)
                .Include(cc => cc.Commands)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
    }
}