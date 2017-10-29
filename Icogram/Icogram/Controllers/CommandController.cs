using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Extensions;
using Icogram.Models.ChatModels;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.Command;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class CommandController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly IUserService _userService;
        private readonly ICrudService<Command> _commandCrudService;
        private readonly ICrudService<Chat> _chatCrudService;

        public CommandController(IViewModelBuilder viewModelBuilder, IUserService userService, ICrudService<Command> companyCrudService, ICrudService<Chat> chatCrudService)
        {
            _viewModelBuilder = viewModelBuilder;
            _userService = userService;
            _commandCrudService = companyCrudService;
            _chatCrudService = chatCrudService;
        }

        public async Task<ActionResult> MyCommands()
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var model = await _viewModelBuilder.GetPageViewModelAsync<MyCommandsPageViewModel>();
            var chats = await _chatCrudService.GetAllAsync();
            model.Chats = HttpContext.User.Identity.IsInRole("Customer") ? chats.Where(c => c.CompanyId == user.CompanyId).Where(c => c.IsApproved).ToList() : chats;

            return View(model);
        }

        #region Commands

        public async Task DeleteCommand(int id)
        {
            var command = await _commandCrudService.GetByIdAsync(id);
            await _commandCrudService.DeleteAsync(command);
        }

        public async Task UpdateMyCommand(UpdateMyCommandViewModel model)
        {
            var command = await _commandCrudService.GetByIdAsync(model.Id);
            command.CommandName = model.CommandName;
            command.IsCommandShowInList = model.IsCommandShowInList;
            command.ActionMessage = model.ActionMessage;
            await _commandCrudService.UpdateAsync(command);
        }

        public async Task CreateMyCommand(CreateMyCommandViewModel model)
        {
            await _commandCrudService.CreateAsync(new Command
            {
                CommandName = model.CommandName,
                ActionMessage = model.ActionMessage,
                ChatId = model.ChatId,
                IsCommandShowInList =  model.IsCommandShowInList
            });
        }
        #endregion
    }
}