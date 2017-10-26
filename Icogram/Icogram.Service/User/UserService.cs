using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Icogram.DataAccessLayer.EntityFramework.Identity;
using Icogram.DataAccessLayer.Interfaces;
using Icogram.Models.UserModels;
using Icogram.ViewModels.User;

namespace Icogram.Service.User
{
    public class UserService : IUserService
    {
        private readonly IIcogramUnitOfWork _icogramUnitOfWork;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IMapper _mapper;


        public UserService(IIcogramUnitOfWork icogramUnitOfWork, IMapper mapper)
        {
            _icogramUnitOfWork = icogramUnitOfWork;
            _mapper = mapper;
            _applicationUserManager = _icogramUnitOfWork.UserManager;
        }


        public async Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return await _applicationUserManager.FindByNameAsync(username);
        }

        public async Task ResetPasswordAsync(ResetPasswordViewModel model)
        {
            await _applicationUserManager.RemovePasswordAsync(model.UserId);
            await _applicationUserManager.AddPasswordAsync(model.UserId, model.NewPassword);
        }

        public async Task CreateAsync(UserCrendentialsViewModel userCrendentials)
        {
            var result = await _applicationUserManager.CreateAsync(new ApplicationUser {UserName = userCrendentials.UserName, CompanyId = userCrendentials.CompanyId}, userCrendentials.Password);
            if (result.Succeeded)
            {
                var user = await _applicationUserManager.FindByNameAsync(userCrendentials.UserName);
                await _applicationUserManager.AddToRoleAsync(user.Id, userCrendentials.Role);
            }
        }

        public async Task UpdateUserProfileAsync(UpdateUserViewModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            user.CompanyId = model.CompanyId;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            await _applicationUserManager.UpdateAsync(user);
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            await _applicationUserManager.DeleteAsync(user);
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            var users = await _applicationUserManager.Users
                .Include(u =>u.Company)
                .ToListAsync();

            return users;
        }

        public async Task<List<ApplicationUser>> GetAllByCompanyAsync(int id)
        {
            var users = await _applicationUserManager.Users
                .Include(u => u.Company)
                .Where( u => u.CompanyId == id)
                .ToListAsync();

            return users;
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            var user = await _applicationUserManager.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return user;
        }

        public async Task<ApplicationUser> GetByUserIdAsync(string userId)
        {
            var user = await _applicationUserManager.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
    }
}
