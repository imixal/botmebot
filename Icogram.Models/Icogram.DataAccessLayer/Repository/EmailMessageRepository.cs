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


        public override Task<List<EmailMessage>> GetAll()
        {
            return GetAllQuery()
                .Include(em => em.Sender)
                .Include(em => em.Template)
                .ToListAsync();
        }

        public override Task<EmailMessage> GetById(int id)
        {
            return GetAllQuery()
                .Include(em => em.Sender)
                .Include(em => em.Template)
                .FirstOrDefaultAsync(em => em.Id == id);
        }
    }
}