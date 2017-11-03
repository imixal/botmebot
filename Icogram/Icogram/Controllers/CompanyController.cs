using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Models.CompanyModels;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.CompanyModels;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly IUserService _userService;
        private readonly ICrudService<Company> _companyCrudService;


        public CompanyController(IViewModelBuilder viewModelBuilder, IUserService userService, ICrudService<Company> companyCrudService)
        {
            _viewModelBuilder = viewModelBuilder;
            _userService = userService;
            _companyCrudService = companyCrudService;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> List()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<CompanyPageViewModel>();
            model.Companies = await _companyCrudService.GetAllAsync();
            model.Users = await _userService.GetAllAsync();

            return View(model);
        }

        #region Commands

        [Authorize(Roles = "Admin, Manager")]
        public async Task EditCommand(Company company)
        {
            if (company.Id == 0)
            {
                await _companyCrudService.CreateAsync(company);
            }
            else
            {
                var oldCompany = await _companyCrudService.GetByIdAsync(company.Id);
                oldCompany.Name = company.Name;
                oldCompany.Description = company.Description;
                oldCompany.End = company.End;
                oldCompany.IsAntiSpamModuleActivated = company.IsAntiSpamModuleActivated;
                oldCompany.IsCommandModuleActivated = company.IsCommandModuleActivated;
                oldCompany.IsCustomMessageModuleActivated = company.IsCustomMessageModuleActivated;
                oldCompany.IsWelcomeMessageModuleActivated = company.IsWelcomeMessageModuleActivated;
                oldCompany.Price = company.Price;

                await _companyCrudService.UpdateAsync(oldCompany);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task DeleteCommand(int id)
        {
            var company = await _companyCrudService.GetByIdAsync(id);
            await _companyCrudService.DeleteAsync(company);
        }

        #endregion
    }
}