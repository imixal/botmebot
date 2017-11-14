using System.Linq;
using System.Threading.Tasks;
using Icogram.Models.ChatModels;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Service;
using System.Web.Mvc;
using Icogram.Models.CompanyModels;
using Icogram.Telegram.BotHandler;
using Icogram.ViewModels.Chat;

namespace Icogram.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly IUserService _userService;
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly ICrudService<Company> _companyCrudService;
        private readonly IBotHandler _botHandler;


        public ChatController(ICrudService<Chat> commandCrudService, IViewModelBuilder viewModelBuilder, IUserService userService, IBotHandler botHandler, ICrudService<Company> companyCrudService)
        {
            _chatCrudService = commandCrudService;
            _viewModelBuilder = viewModelBuilder;
            _userService = userService;
            _botHandler = botHandler;
            _companyCrudService = companyCrudService;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> List()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<ChatsPageViewModel>();
            model.Chats = await _chatCrudService.GetAllAsync();
            model.Companies = await _companyCrudService.GetAllAsync();

            return View(model);
        }

        public async Task<ActionResult> MyChats()
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var model = await _viewModelBuilder.GetPageViewModelAsync<ChatsPageViewModel>();
            var chats = await _chatCrudService.GetAllAsync();
            model.Chats = chats.Where(c => c.CompanyId.HasValue).Where(c => c.CompanyId == user.CompanyId && c.IsApproved).ToList();

            return View(model);
        }

        #region Commands

        [Authorize(Roles = "Admin, Manager")]
        public async Task EditCommand(Chat chat)
        {
            if (chat.Id == 0)
            {
                await _chatCrudService.CreateAsync(chat);
            }
            else
            {
                var oldChat = await _chatCrudService.GetByIdAsync(chat.Id);
                oldChat.CommandTimeOut = chat.CommandTimeOut;
                oldChat.IsApproved = chat.IsApproved;
                oldChat.WelcomeUserMessage = chat.WelcomeUserMessage;
                oldChat.CompanyId = chat.CompanyId;

                await _chatCrudService.UpdateAsync(oldChat);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task DeleteCommand(int id)
        {
            await _botHandler.LeaveChatAsync(id);
            var chat = await _chatCrudService.GetByIdAsync(id);
            await _chatCrudService.DeleteAsync(chat);
        }

        public async Task UpdateMyChat(Chat newChat)
        {
            var chat = await _chatCrudService.GetByIdAsync(newChat.Id);
            chat.WelcomeUserMessage = newChat.WelcomeUserMessage;
            chat.CommandTimeOut = newChat.CommandTimeOut;
            chat.IsNeededToDeleteLeaveUserMessage = newChat.IsNeededToDeleteLeaveUserMessage;
            chat.IsNeededToDeleteNewUserMessage = newChat.IsNeededToDeleteNewUserMessage;

            await _chatCrudService.UpdateAsync(chat);
        }

        public async Task UpdateChatCommand(int id)
        {
            await _botHandler.UpdateChatFieldsAsync(id);
        }

        public async Task SendMessageCommand(int icogramChatId, string message)
        {
            await _botHandler.SendMessageAsync(icogramChatId, message);
        }

        #endregion
    }
}