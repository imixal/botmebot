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


        public override Task<List<Command>> GetAll()
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(c => c.Bot)
                .Include(c => c.Type)
                .ToListAsync();
        }

        public override Task<Command> GetById(int id)
        {
            return GetAllQuery()
                .Include(c => c.Bot)
                .Include(c => c.Type)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override Task<Command> GetByIdAsNoTracking(int id)
        {
            return GetAllQuery()
                .AsNoTracking()
                .Include(c => c.Bot)
                .Include(c => c.Type)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}