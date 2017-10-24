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

        public override Task<List<ChatCommand>> GetAll()
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Chat)
                .Include(cc => cc.Command)
                .ToListAsync();
        }

        public override Task<ChatCommand> GetById(int id)
        {
            return GetAllQuery()
                .Include(cc => cc.Chat)
                .Include(cc => cc.Command)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public override Task<ChatCommand> GetByIdAsNoTracking(int id)
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Chat)
                .Include(cc => cc.Command)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
    }
}