using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.Models.Abstract;

namespace Service
{
    public class CrudService<T> : ICrudService<T> where T : Entity
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T> _repository;

        public CrudService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T>();
        }


        public async Task CreateAsync(T entity)
        {
            _repository.Create(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> GetByIdAsNoTrackingAsync(int id)
        {
            return await _repository.GetByIdAsNoTrackingAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}