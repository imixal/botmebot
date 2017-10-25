using System.Threading.Tasks;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Icogram.DataAccessLayer.EntityFramework.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        {
        }


        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Users.Include(u => u.Company)
                .Include(u => u.Roles)
                .Include(u => u.Claims)
                .Include(u => u.Logins)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}