using System.Collections.Generic;
using Icogram.Models.Abstract;

namespace DataAccessLayer.Async
{
    public interface IRepository<T> where T : Entity 
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        T GetById(int id);

        IReadOnlyCollection<T> GetAll();
    }
}