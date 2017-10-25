using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.Dashboard;

namespace Icogram.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;


        public DashboardController(IViewModelBuilder viewModelBuilder)
        {
            _viewModelBuilder = viewModelBuilder;
        }


        public async Task<ActionResult> Index()
        {
            var viewModel =await _viewModelBuilder.GetPageViewModelAsync<DashboardViewModel>();

            return View(viewModel);
        }
    }
}