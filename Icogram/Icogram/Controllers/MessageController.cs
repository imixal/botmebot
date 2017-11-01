using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Extensions;
using Icogram.Models.ChatModels;
using Icogram.Service.User;
using Icogram.Telegram.BotHandler;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.Chat;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IBotHandler _botHandler;
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly IUserService _userService;


        public MessageController(IBotHandler botHandler, IViewModelBuilder viewModelBuilder, ICrudService<Chat> chatCrudService, IUserService userService)
        {
            _botHandler = botHandler;
            _viewModelBuilder = viewModelBuilder;
            _chatCrudService = chatCrudService;
            _userService = userService;
        }


        public async Task<ActionResult> Send()
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var model = await _viewModelBuilder.GetPageViewModelAsync<ChatsPageViewModel>();
            model.Chats = await _chatCrudService.GetAllAsync();
            if (HttpContext.User.Identity.IsInRole("Customer"))
            {
                model.Chats = model.Chats.Where(c => c.CompanyId == user.CompanyId).ToList();
            }

            return View(model);
        }

        #region Command

        public async Task SendMessageCommand(string text, int chatId)
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var chat = await _chatCrudService.GetByIdAsNoTrackingAsync(chatId);

            if (user.CompanyId == chat.CompanyId)
            {
                await _botHandler.SendMessageAsync(chatId, text);
            }
        }

        #endregion
    }
}