using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.BotModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class CustomerBotRepository : Repository<CustomerBot>
    {
        public CustomerBotRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<CustomerBot>> GetAll()
        {
            return GetAllQuery()
                .Include(cb => cb.Company)
                .ToListAsync();
        }

        public override Task<CustomerBot> GetById(int id)
        {
            return GetAllQuery()
                .Include(cb => cb.Company)
                .FirstOrDefaultAsync(cb => cb.Id == id);

        }
    }
}