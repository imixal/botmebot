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


        public override Task<List<CompanyTarif>> GetAll()
        {
            return GetAllQuery()
                .Include(ct => ct.Company)
                .ToListAsync();
        }

        public override Task<CompanyTarif> GetById(int id)
        {
            return GetAllQuery()
                .Include(ct => ct.Company)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }
    }
}