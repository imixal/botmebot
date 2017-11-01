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


        public override async Task<List<Company>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(c => c.Users)
                .Include(c => c.Chats)
                .Include(c => c.Payments)
                .ToListAsync();
        }

        public override async Task<Company> GetByIdAsync(int id)
        {
            return await DbContext.Set<Company>()
                .Include(c => c.Chats)
                .Include(c => c.Users)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<Company> GetByIdAsNoTrackingAsync(int id)
        {
            return await DbContext.Set<Company>()
                .AsNoTracking()
                .Include(c => c.Chats)
                .Include(c => c.Users)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}