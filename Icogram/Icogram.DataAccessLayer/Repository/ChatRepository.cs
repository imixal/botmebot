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

        public override Task<List<Chat>> GetAll()
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Company)
                .ToListAsync();
        }

        public override Task<Chat> GetById(int id)
        {
            return GetAllQuery()
                .Include(cc => cc.Company)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public override Task<Chat> GetByIdAsNoTracking(int id)
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Company)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
    }
}