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

        public override Task<List<EmailTemplate>> GetAll()
        {
            return GetAllQuery()
                .Include(et => et.Creator)
                .Include(et => et.Variables)
                .ToListAsync();
        }

        public override Task<EmailTemplate> GetById(int id)
        {
            return GetAllQuery()
                .Include(et => et.Creator)
                .Include(et => et.Variables)
                .FirstOrDefaultAsync(et => et.Id == id);
        }
    }
}