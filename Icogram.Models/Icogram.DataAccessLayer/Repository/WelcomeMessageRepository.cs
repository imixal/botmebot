using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.WelcomeMessageModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class WelcomeMessageRepository : Repository<WelcomeMessage>
    {
        public WelcomeMessageRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override Task<List<WelcomeMessage>> GetAll()
        {
            return GetAllQuery()
                .Include(cm => cm.Bot)
                .ToListAsync();
        }

        public override Task<WelcomeMessage> GetById(int id)
        {
            return GetAllQuery()
                .Include(cm => cm.Bot)
                .FirstOrDefaultAsync(wm => wm.Id == id);
        }
    }
}