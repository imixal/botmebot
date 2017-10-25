using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.CompanyModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class CompanyTarifRepository : Repository<CompanyTarif>
    {
        public CompanyTarifRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<List<CompanyTarif>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(ct => ct.Company)
                .ToListAsync();
        }

        public override async Task<CompanyTarif> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(ct => ct.Company)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public override async Task<CompanyTarif> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(ct => ct.Company)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }
    }
}