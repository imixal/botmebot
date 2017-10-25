using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Icogram.Service.User;
using Icogram.ViewModels.Layout;
using Icogram.ViewModels.User;

namespace Icogram.ViewModelBuilder
{
    public class ViewModelBuilder : IViewModelBuilder
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public ViewModelBuilder(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<TViewModel> GetPageViewModelAsync<TViewModel>() where TViewModel : LayoutViewModel, new()
        {
            var user = await _userService.GetUserByNameAsync(HttpContext.Current.User.Identity.Name);
            var userProfile = _mapper.Map<UserProfileViewModel>(user);
            var viewModel = new TViewModel {UserProfileViewModel = userProfile};

            return viewModel;
        }
    }
}