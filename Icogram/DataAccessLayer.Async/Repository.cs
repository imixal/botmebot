using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Icogram.Models.Abstract;

namespace DataAccessLayer.Async
{
    public class Repository<T> : IDisposable, IRepository<T> where T : Entity 
    {
        protected readonly DbContext DbContext;


        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }


        public virtual void Create(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Added;
        }

        public virtual void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual async Task<T> GetByIdAsNoTrackingAsync(int id)
        {
            return await GetAllQuery().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await GetAllQuery().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var user = await DbContext.Set<T>().FindAsync(id);

            return user;
        }

        protected IQueryable<T> GetAllQuery()
        {
            return DbContext.Set<T>();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}