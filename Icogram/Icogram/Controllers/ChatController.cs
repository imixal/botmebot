using System.Threading.Tasks;
using Icogram.Models.ChatModels;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Service;
using System.Web.Mvc;
using Icogram.ViewModels.Chat;

namespace Icogram.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly IUserService _userService;
        private readonly ICrudService<Chat> _chatCrudService;


        public ChatController(ICrudService<Chat> commandCrudService, IViewModelBuilder viewModelBuilder, IUserService userService)
        {
            _chatCrudService = commandCrudService;
            _viewModelBuilder = viewModelBuilder;
            _userService = userService;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> List()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<ChatsPageViewModel>();
            model.Chats = await _chatCrudService.GetAllAsync();

            return View(model);
        }

        public async Task<ActionResult> MyChats()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<ChatsPageViewModel>();


            return View();
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
                await _chatCrudService.UpdateAsync(chat);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task DeleteCommand(int id)
        {
            var chat = await _chatCrudService.GetByIdAsync(id);
            await _chatCrudService.DeleteAsync(chat);
            //bot leave chat
        }

        #endregion
    }
}