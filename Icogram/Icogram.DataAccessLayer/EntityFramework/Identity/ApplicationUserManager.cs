using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity;

namespace Icogram.DataAccessLayer.EntityFramework.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        {
        }
    }
}