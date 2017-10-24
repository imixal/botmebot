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


        public override Task<List<CustomMessage>> GetAll()
        {
            return GetAllQuery()
                .Include(cm => cm.Bot)
                .ToListAsync();
        }

        public override Task<CustomMessage> GetById(int id)
        {
            return GetAllQuery()
                .Include(cm => cm.Bot)
                .FirstOrDefaultAsync(cm=> cm.Id == id);
        }
    }
}