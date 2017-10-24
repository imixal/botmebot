using DataAccessLayer.Async;
using Icogram.DataAccessLayer.EntityFramework.Identity;

namespace Icogram.DataAccessLayer.Interfaces
{
    public interface IIcogramUnitOfWork : IUnitOfWork
    {
        ApplicationUserManager UserManager { get; }

        ApplicationRoleManager RoleManager { get; }
    }
}