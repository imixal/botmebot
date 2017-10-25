using System.Collections.Generic;
using System.Threading.Tasks;
using Icogram.Models.Abstract;

namespace Service
{
    public interface ICrudService<T> where T : Entity
    {
        Task CreateAsync(T entity);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsNoTrackingAsync(int id);

        Task<List<T>> GetAllAsync();

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}