using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DbContext;
using Icogram.Models.Payments;

namespace Icogram.DataAccessLayer.Repository
{
    public class PaymentRepository : Repository<Payment>
    {
        public PaymentRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<List<Payment>> GetAllAsync()
        {
            return await GetAllQuery()
                .Include(p => p.Company)
                .Include(p => p.PaymentType)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Payment> GetByIdAsync(int id)
        {
            return await GetAllQuery()
                .Include(p => p.Company)
                .Include(p => p.PaymentType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<Payment> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery()
                .Include(p => p.Company)
                .Include(p => p.PaymentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}