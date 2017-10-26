using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Models.CompanyModels;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.User;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly IUserService _userService;
        private readonly ICrudService<Company> _companyCrudService;


        public UserController(IViewModelBuilder viewModelBuilder, IUserService userService, ICrudService<Company> companyCrudService)
        {
            _viewModelBuilder = viewModelBuilder;
            _userService = userService;
            _companyCrudService = companyCrudService;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> List()
        {
            var viewModel = await _viewModelBuilder.GetPageViewModelAsync<UserListViewModel>();
            viewModel.Companies = await _companyCrudService.GetAllAsync();
            viewModel.Users = await _userService.GetAllAsync();

            return View(viewModel);
        }

        public async Task<ActionResult> MyUserProfile()
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var viewModel = await _viewModelBuilder.GetPageViewModelAsync<MyUserProfilePageViewModel>();
            viewModel.FirstName = user.FirstName;
            viewModel.LastName = user.LastName;

            return View(viewModel);
        }

        #region Commands

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> CreateCommand(UserCrendentialsViewModel viewModel)
        {
            await _userService.CreateAsync(viewModel);

            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> EditCommand(UpdateUserViewModel viewModel)
        {
            await _userService.UpdateUserProfileAsync(viewModel);

            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteCommand(string userId)
        {
            await _userService.DeleteAsync(userId);

            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> ResetPasswwordCommand(ResetPasswordViewModel viewModel)
        {
            var user = await _userService.GetByUserNameAsync(viewModel.UserId);
            viewModel.UserId = user.Id;
            await _userService.ResetPasswordAsync(viewModel);

            return RedirectToAction("List");
        }

        public async Task<ActionResult> ResetMyPasswordCommand(string newPassword)
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var viewModel = new ResetPasswordViewModel {UserId = user.Id, NewPassword =  newPassword};
            await _userService.ResetPasswordAsync(viewModel);

            return RedirectToAction("MyUserProfile");
        }

        public async Task<ActionResult> UpdateMyUserProfileCommand(string firstName, string lastName)
        {
            var currentUser = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var updateUserViewModel = new UpdateUserViewModel
            {
                UserId = currentUser.Id,
                LastName = lastName,
                FirstName = firstName
            };
            if (currentUser.CompanyId.HasValue)
            {
                updateUserViewModel.CompanyId = currentUser.CompanyId.Value;
            }
            await _userService.UpdateUserProfileAsync(updateUserViewModel);

            return RedirectToAction("MyUserProfile");
        }
        #endregion
    }
}