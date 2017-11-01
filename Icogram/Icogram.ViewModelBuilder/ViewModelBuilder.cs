using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Icogram.Models.ResourcesModels;
using Icogram.Service.User;
using Icogram.ViewModels.Layout;
using Icogram.ViewModels.User;
using Service;

namespace Icogram.ViewModelBuilder
{
    public class ViewModelBuilder : IViewModelBuilder
    {
        private readonly IUserService _userService;
        private readonly ICrudService<Resource> _resourceCrudService;
        private readonly IMapper _mapper;


        public ViewModelBuilder(IUserService userService, IMapper mapper, ICrudService<Resource> resourceCrudService)
        {
            _userService = userService;
            _mapper = mapper;
            _resourceCrudService = resourceCrudService;
        }


        public async Task<TViewModel> GetPageViewModelAsync<TViewModel>() where TViewModel : LayoutViewModel, new()
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.Current.User.Identity.Name);
            var userProfile = _mapper.Map<UserProfileViewModel>(user);
            var viewModel = new TViewModel
            {
                UserProfileViewModel = userProfile,
                Resources = await _resourceCrudService.GetAllAsync()
            };

            return viewModel;
        }

        public async Task<LayoutViewModel> GetPageViewModelAsync()
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.Current.User.Identity.Name);
            var userProfile = _mapper.Map<UserProfileViewModel>(user);
            var viewModel = new LayoutViewModel
            {
                UserProfileViewModel = userProfile,
                Resources = await _resourceCrudService.GetAllAsync()
            };

            return viewModel;
        }
    }
}