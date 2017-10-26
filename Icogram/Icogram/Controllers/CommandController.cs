﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.Command;
using Microsoft.AspNet.Identity;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class CommandController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly IUserService _userService;
        private readonly ICrudService<Command> _commandCrudService;
        private readonly ICrudService<ChatCommand> _chatCommandCrudService;

        public CommandController(IViewModelBuilder viewModelBuilder, IUserService userService, ICrudService<Command> companyCrudService, ICrudService<ChatCommand> chatCommandCrudService)
        {
            _viewModelBuilder = viewModelBuilder;
            _userService = userService;
            _commandCrudService = companyCrudService;
            _chatCommandCrudService = chatCommandCrudService;
        }


        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> List()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<CommandPageViewModel>();
            model.Commands = await _commandCrudService.GetAllAsync();

            return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> CustomerChatCommands()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<CustomerChatCommandsPageViewModel>();
            model.ChatCommands = await _chatCommandCrudService.GetAllAsync();
            model.Commands = await _commandCrudService.GetAllAsync();

            return View(model);
        }

        public async Task<ActionResult> ChatCommands()
        {
            var user = await _userService.GetUserByNameAsync(HttpContext.User.Identity.GetUserName());
            var model = await _viewModelBuilder.GetPageViewModelAsync<CustomerChatCommandsPageViewModel>();
            model.ChatCommands = await _chatCommandCrudService.GetAllAsync();
            model.Commands = await _commandCrudService.GetAllAsync();

            return View(model);
        }

        #region Commands

        [Authorize(Roles = "Admin, Manager")]
        public async Task EditCommand(Command command)
        {
            if (command.Id == 0)
            {
                await _commandCrudService.CreateAsync(command);
            }
            else
            {
                await _commandCrudService.UpdateAsync(command);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task DeleteCommand(int id)
        {
            var command = await _commandCrudService.GetByIdAsync(id);
            await _commandCrudService.DeleteAsync(command);
        }

        public async Task ChatCommandEditCommand(ChatCommand chatCommand)
        {
            if (chatCommand.Id == 0)
            {
                await _chatCommandCrudService.CreateAsync(chatCommand);
            }
            else
            {
                await _chatCommandCrudService.UpdateAsync(chatCommand);
            }
        }

        public async Task ChatCommandDeleteCommand(int id)
        {
            var chatCommand = await _chatCommandCrudService.GetByIdAsync(id);
            await _chatCommandCrudService.DeleteAsync(chatCommand);
        }
        #endregion
    }
}