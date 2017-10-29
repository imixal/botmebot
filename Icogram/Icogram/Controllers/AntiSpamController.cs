using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Extensions;
using Icogram.Models.ChatModels;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.AntiSpam;
using Service;

namespace Icogram.Controllers
{
    public class AntiSpamController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly ICrudService<AntiSpamSetting> _settingsCrudService;
        private readonly ICrudService<SuspiciousUser> _suspiciousUsersCrudService;
        private readonly ICrudService<WhiteLink> _whiteLinksCrudService;
        private readonly IViewModelBuilder _viewModelBuilder;


        public AntiSpamController(IUserService userService, ICrudService<Chat> chatCrudService, ICrudService<SuspiciousUser> suspiciousUserCrudService, ICrudService<AntiSpamSetting> settingsCrudService, ICrudService<WhiteLink> whiteLinksCrudService, IViewModelBuilder viewModelBuilder)
        {
            _userService = userService;
            _chatCrudService = chatCrudService;
            _suspiciousUsersCrudService = suspiciousUserCrudService;
            _settingsCrudService = settingsCrudService;
            _whiteLinksCrudService = whiteLinksCrudService;
            _viewModelBuilder = viewModelBuilder;
        }


        public async Task<ActionResult> Settings()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<SettingsPageViewModel>();
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            model.Chats = await _chatCrudService.GetAllAsync();
            model.Settings = await _settingsCrudService.GetAllAsync();

            if (!HttpContext.User.Identity.IsInRole("Customer")) return View(model);
            model.Settings = model.Settings.Where(s => s.Chat.CompanyId == user.CompanyId && s.Chat.IsApproved).ToList();
            model.Chats = model.Chats.Where(c => c.CompanyId == user.CompanyId && c.IsApproved).ToList();

            return View(model);
        }

        public async Task<ActionResult> WhiteLinks()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<WhiteLinkPageViewModel>();
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            model.WhiteLinks = await _whiteLinksCrudService.GetAllAsync();
            model.Chats = await _chatCrudService.GetAllAsync();

            if (!HttpContext.User.Identity.IsInRole("Customer")) return View(model);
            model.WhiteLinks = model.WhiteLinks.Where(wl => wl.Chat.CompanyId == user.CompanyId).ToList();
            model.Chats = model.Chats.Where(c => c.IsApproved && c.CompanyId == user.CompanyId).ToList();

            return View(model);
        }

        public async Task<ActionResult> SuspiciousUsers()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<SuspiciousUsersPageViewModel>();
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            model.SuspiciousUsers = await _suspiciousUsersCrudService.GetAllAsync();

            if (!HttpContext.User.Identity.IsInRole("Customer")) return View(model);
            model.SuspiciousUsers = model.SuspiciousUsers.Where(su => su.Chat.CompanyId == user.CompanyId).ToList();

            return View(model);
        }
    }
}