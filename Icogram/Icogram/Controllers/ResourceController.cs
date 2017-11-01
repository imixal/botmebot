using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Models.ResourcesModels;
using Icogram.ViewModelBuilder;
using Service;

namespace Icogram.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class ResourceController : Controller
    {
        private readonly ICrudService<Resource> _resourceCrudService;
        private readonly IViewModelBuilder _viewModelBuilder;

        public ResourceController(ICrudService<Resource> resourceCrudService, IViewModelBuilder viewModelBuilder)
        {
            _resourceCrudService = resourceCrudService;
            _viewModelBuilder = viewModelBuilder;
        }


        public async Task<ActionResult> List()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync();
            return View(model);
        }

        #region Commands

        [ValidateInput(false)]
        public async Task CreateResourceCommand(Resource resource)
        {
            resource.EnglishValue = resource.DefaultValue;
            await _resourceCrudService.CreateAsync(resource);
        }

        [ValidateInput(false)]
        public async Task UpdateResourceCommand(Resource newResource)
        {
            var resource = await _resourceCrudService.GetByIdAsync(newResource.Id);
            resource.DefaultValue = newResource.DefaultValue;
            resource.EnglishValue = newResource.DefaultValue;
            resource.Name = newResource.Name;

            await _resourceCrudService.UpdateAsync(resource);
        }

        public async Task DeleteResourceCommand(int id)
        {
            var resource = await _resourceCrudService.GetByIdAsync(id);
            await _resourceCrudService.DeleteAsync(resource);
        }

        #endregion
    }
}