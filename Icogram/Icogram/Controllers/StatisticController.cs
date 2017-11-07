using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Extensions;
using Icogram.Models.ChatModels;
using Icogram.Service.ChatStatistic;
using Icogram.Service.User;
using Icogram.Telegram.BotHandler;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.ChatStatistic;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly IChatStatisticService _chatStatisticService;
        private readonly IUserService _userService;


        public StatisticController(IViewModelBuilder viewModelBuilder, ICrudService<Chat> chatCrudService, IChatStatisticService chatStatisticService, IUserService userService)
        {
            _viewModelBuilder = viewModelBuilder;
            _chatCrudService = chatCrudService;
            _chatStatisticService = chatStatisticService;
            _userService = userService;
        }


        public async Task<ActionResult> MyStatistic(int chatId = 0)
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<MyStatisticPageViewModel>();
            var chats = await _chatCrudService.GetAllAsync();
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            model.Chats = HttpContext.User.Identity.IsInRole("Customer") ? chats.Where(c => c.CompanyId == user.CompanyId).Where(c => c.IsApproved).ToList() : chats.Where(c => c.IsApproved && c.CompanyId.HasValue).ToList();
            model.IsChatsEmpty = model.Chats == null;

            if (chatId == 0)
            {
                if (!model.IsChatsEmpty)
                {
                    model.ChatStats = await _chatStatisticService.GetChatStatistic(model.Chats.First().Id);
                    model.ChatStats = model.ChatStats.OrderByDescending(cs => cs.Date).Take(10).OrderBy(cs => cs.Date).ToList();
                }
            }
            else
            {
                if (!model.IsChatsEmpty)
                {
                    model.ChatStats = await _chatStatisticService.GetChatStatistic(chatId);
                    model.ChatStats = model.ChatStats.OrderByDescending(cs => cs.Date).Take(10).OrderBy(cs=>cs.Date).ToList();
                }
            }

            return View(model);
        }
    }
}