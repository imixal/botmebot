using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Icogram.Models.Abstract;

namespace DataAccessLayer.Async
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDictionary<Type, object> _repositories;

        protected readonly DbContext DbContext;


        public UnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }


        public void Dispose()
        {
            DbContext.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            var repository = CreateRepository<TEntity>();
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        protected virtual IRepository<TEntity> CreateRepository<TEntity>() where TEntity : Entity
        {
            return new Repository<TEntity>(DbContext);
        }
    }
}