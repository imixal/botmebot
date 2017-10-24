using System;
using System.Threading.Tasks;
using Icogram.Models.Abstract;

namespace DataAccessLayer.Async
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : Entity;

        Task SaveChangesAsync();
    }
}