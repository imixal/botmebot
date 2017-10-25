using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.EmailModels;

namespace Icogram.DataAccessLayer.Repository
{
    public class EmailMessageRepository : Repository<EmailMessage>
    {
        public EmailMessageRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<List<EmailMessage>> GetAllAsync()
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(em => em.Sender)
                .Include(em => em.Template)
                .ToListAsync();
        }

        public override async Task<EmailMessage> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(em => em.Sender)
                .Include(em => em.Template)
                .FirstOrDefaultAsync(em => em.Id == id);
        }

        public override async Task<EmailMessage> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .AsNoTracking()
                .Include(em => em.Sender)
                .Include(em => em.Template)
                .FirstOrDefaultAsync(em => em.Id == id);
        }
    }
}