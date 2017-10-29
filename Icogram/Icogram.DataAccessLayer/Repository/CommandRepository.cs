using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.CommandModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class CommandRepository : Repository<Command>
    {
        public CommandRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }


        public override Task<List<Command>> GetAllAsync()
        {
            return GetAllQuery()
                .Include(c=>c.Chat)
                .AsNoTracking()
                .ToListAsync();
        }

        public override Task<Command> GetByIdAsNoTrackingAsync(int id)
        {
            return GetAllQuery()
                .Include(c => c.Chat)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override Task<Command> GetByIdAsync(int id)
        {
            return GetAllQuery()
                .Include(c => c.Chat)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}