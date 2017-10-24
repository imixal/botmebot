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

        public virtual Task<List<T>> GetAll()
        {
            return GetAllQuery().ToListAsync();
        }

        public virtual Task<T> GetById(int id)
        {
            var user = DbContext.Set<T>().FindAsync(id);

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