using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.CompanyModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class CompanyChatRepository : Repository<CompanyChat>
    {
        public CompanyChatRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<CompanyChat>> GetAll()
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Company)
                .ToListAsync();
        }

        public override Task<CompanyChat> GetById(int id)
        {
            return GetAllQuery()
                .Include(cc => cc.Company)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public override Task<CompanyChat> GetByIdAsNoTracking(int id)
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(cc => cc.Company)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
    }
}