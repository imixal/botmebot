using System.Collections.Generic;
using System.Threading.Tasks;
using Icogram.Models.UserModels;
using Icogram.ViewModels.User;

namespace Icogram.Service.User
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByNameAsync(string username);

        Task ResetPasswordAsync(ResetPasswordViewModel model);

        Task CreateAsync(UserCrendentialsViewModel userCrendentials);

        Task UpdateUserProfileAsync(UpdateUserViewModel model);

        Task DeleteAsync(string userId);

        Task<List<ApplicationUser>> GetAllAsync();

        Task<List<ApplicationUser>> GetAllByCompanyAsync(int id);

        Task<ApplicationUser> GetByUserNameAsync(string userName);

        Task<ApplicationUser> GetByUserIdAsync(string userId);
    }
}