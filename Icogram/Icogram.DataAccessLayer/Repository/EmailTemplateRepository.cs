using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.EmailModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class EmailTemplateRepository : Repository<EmailTemplate>
    {
        public EmailTemplateRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<List<EmailTemplate>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(et => et.Creator)
                .Include(et => et.Variables)
                .ToListAsync();
        }

        public override async Task<EmailTemplate> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(et => et.Creator)
                .Include(et => et.Variables)
                .FirstOrDefaultAsync(et => et.Id == id);
        }

        public override async Task<EmailTemplate> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(et => et.Creator)
                .Include(et => et.Variables)
                .FirstOrDefaultAsync(et => et.Id == id);
        }
    }
}