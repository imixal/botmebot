using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.CompanyModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class CompanyRepository : Repository<Company>
    {
        public CompanyRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override Task<List<Company>> GetAll()
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(c => c.Chats)
                .Include(c => c.Tarif)
                .Include(c => c.TelegramBots)
                .ToListAsync();
        }

        public override Task<Company> GetById(int id)
        {
            return DbContext.Set<Company>()
                .Include(c => c.Chats)
                .Include(c => c.Tarif)
                .Include(c => c.TelegramBots)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override Task<Company> GetByIdAsNoTracking(int id)
        {
            return DbContext.Set<Company>()
                .AsNoTracking()
                .Include(c => c.Chats)
                .Include(c => c.Tarif)
                .Include(c => c.TelegramBots)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}