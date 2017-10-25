using System.Collections.Generic;
using System.Threading.Tasks;
using Icogram.Models.Abstract;

namespace DataAccessLayer.Async
{
    public interface IRepository<T> where T : Entity 
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsNoTrackingAsync(int id);

        Task<List<T>> GetAllAsync();
    }
}