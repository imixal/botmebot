using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Extensions;
using Icogram.Models.ChatModels;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.Service.User;
using Icogram.Telegram.BotHandler;
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
        private readonly IBotHandler _botHandler;


        public AntiSpamController(IUserService userService, ICrudService<Chat> chatCrudService, ICrudService<SuspiciousUser> suspiciousUserCrudService, ICrudService<AntiSpamSetting> settingsCrudService, ICrudService<WhiteLink> whiteLinksCrudService, IViewModelBuilder viewModelBuilder, IBotHandler botHandler)
        {
            _userService = userService;
            _chatCrudService = chatCrudService;
            _suspiciousUsersCrudService = suspiciousUserCrudService;
            _settingsCrudService = settingsCrudService;
            _whiteLinksCrudService = whiteLinksCrudService;
            _viewModelBuilder = viewModelBuilder;
            _botHandler = botHandler;
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

        #region Commands

        public async Task UpdateSettingsCommand(AntiSpamSetting model)
        {
            if (model.Id == 0)
            {
                await _settingsCrudService.CreateAsync(new AntiSpamSetting
                {
                    ChatId = model.ChatId,
                    IsInvertMode = model.IsInvertMode,
                    IsModuleIncluded = model.IsModuleIncluded,
                    IsNeededToBanUser = model.IsNeededToBanUser,
                    NumberOfAttempts = model.NumberOfAttempts,
                    WarningMessage = model.WarningMessage
                });
            }
            else
            {
                var settings = await _settingsCrudService.GetByIdAsync(model.Id);
                settings.IsInvertMode = model.IsInvertMode;
                settings.IsModuleIncluded = model.IsModuleIncluded;
                settings.IsNeededToBanUser = model.IsNeededToBanUser;
                settings.NumberOfAttempts = model.NumberOfAttempts;
                settings.WarningMessage = model.WarningMessage;

                await _settingsCrudService.UpdateAsync(settings);
            }
        }

        public async Task CreateWhiteLinkCommand(WhiteLink model)
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            if (model.Id == 0)
            {
                if (model.ChatId == 0)
                {
                    var chats = await _chatCrudService.GetAllAsync();
                    chats = chats.Where(c => c.CompanyId == user.CompanyId).ToList();
                    foreach (var chat in chats)
                    {
                        await _whiteLinksCrudService.CreateAsync(new WhiteLink {ChatId = chat.Id, Link = model.Link});
                    }
                }
                else
                {
                    await _whiteLinksCrudService.CreateAsync(new WhiteLink { ChatId = model.ChatId, Link = model.Link });
                }
            }
            else
            {
                var link = await _whiteLinksCrudService.GetByIdAsync(model.Id);
                link.ChatId = model.ChatId;
                link.Link = model.Link;

                await _whiteLinksCrudService.UpdateAsync(link);
            }
        }

        public async Task DeleteWhiteLinkCommand(int id)
        {
            var link = await _whiteLinksCrudService.GetByIdAsync(id);
            await _whiteLinksCrudService.DeleteAsync(link);
        }

        public async Task KickCommand(int id)
        {
            var user = await _suspiciousUsersCrudService.GetByIdAsync(id);
            await _botHandler.BanUserAsync(user);
        }

        public async Task UnBanCommand(int id)
        {
            var user = await _suspiciousUsersCrudService.GetByIdAsync(id);
            await _botHandler.UnBanUserAsync(user);
        }

        public async Task ResetAttempts(int id)
        {
            var user = await _suspiciousUsersCrudService.GetByIdAsync(id);
            user.NumberOfAttempts = 0;
            await _suspiciousUsersCrudService.UpdateAsync(user);
        }
        #endregion
    }
}