using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Enums;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Service;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Telegram.Bot.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.String;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler
{
    public class BotHandler : IBotHandler
    {
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly ICrudService<Command> _commandCrudService;
        private readonly ICrudService<AntiSpamSetting> _antiSpamSettingsCrudService;
        private readonly ICrudService<WhiteLink> _whiteLinkCrudService;
        private readonly ICrudService<SuspiciousUser> _suspiciousUserCrudService;
        private readonly TelegramBotClient _telegramBotClient;
        private Chat _chat;

        public BotHandler(ICrudService<Chat> chatCrudService, ICrudService<Command> commandCrudService,
            ICrudService<SuspiciousUser> suspiciousUser, ICrudService<WhiteLink> whiteLinkCrudService,
            ICrudService<AntiSpamSetting> antiSpamSettingsCrudService,
            ICrudService<SuspiciousUser> suspiciousUserCrudService)
        {
            _chatCrudService = chatCrudService;
            _commandCrudService = commandCrudService;
            _whiteLinkCrudService = whiteLinkCrudService;
            _antiSpamSettingsCrudService = antiSpamSettingsCrudService;
            _suspiciousUserCrudService = suspiciousUserCrudService;
            _telegramBotClient = IcogramBot.GetClient();
        }

        public async Task MessageHandler(Update update, Chat chat)
        {
            try
            {
                _chat = chat;
                if (update.Type == UpdateType.MessageUpdate)
                {
                    if (update.Message != null)
                    {
                        if (!IsNullOrEmpty(update.Message.Text))
                        {
                            if (update.Message.Entities != null)
                            {
                                if (update.Message.Entities.Any(e => e.Type == MessageEntityType.BotCommand))
                                {
                                    await ShowListCommandsAsync(update);
                                }
                                if (update.Message.Entities.Any(e => e.Type == MessageEntityType.Url))
                                {
                                    await LinkCheck(update);
                                }
                            }
                            await TryToExecuteCommand(update);
                        }
                        try
                        {
                            if (update.Message.LeftChatMember != null)
                            {
                                await LeaveUserCheck(update);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            await NewUserCheck(update);
                        }
                        catch (Exception)
                        {

                        }
                    }

                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<Chat> GetApprovedChatAsync(long telegramChatId)
        {
            try
            {
            var chats = await _chatCrudService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.TelegramChatId == telegramChatId);
            if (chat != null) return chat.IsApproved ? chat : null;
            var telegramChat = await _telegramBotClient.GetChatAsync(telegramChatId);
            if (telegramChat == null) return null;
            var icogramChat = new Chat
            {
                IsApproved = false,
                TelegramChatId = telegramChatId,
                Title = telegramChat.Title,
                Type = telegramChat.Type.ToString()
            };
            await _chatCrudService.CreateAsync(icogramChat);
            }
            catch (Exception)
            {

            }
            return null;
        }

        public async Task UpdateChatFieldsAsync(int icogramChatId)
        {
            var icogramChat = await _chatCrudService.GetByIdAsync(icogramChatId);
            var telegramChat = await _telegramBotClient.GetChatAsync(icogramChat.TelegramChatId);
            if (telegramChat != null)
            {
                icogramChat.Title = telegramChat.Title;
                icogramChat.Type = telegramChat.Type.ToString();
            }
            else
            {
                icogramChat.Title = "Chat Not Found";
            }

            await _chatCrudService.UpdateAsync(icogramChat);
        }

        public async Task LeaveChatAsync(int icogramChatId)
        {
            try
            {
                var icogramChat = await _chatCrudService.GetByIdAsNoTrackingAsync(icogramChatId);
                await _telegramBotClient.LeaveChatAsync(icogramChat.TelegramChatId);
            }
            catch (Exception)
            {

            }
        }

        public async Task SendMessageAsync(int icogramChatId, string message)
        {
            var icogramChat = await _chatCrudService.GetByIdAsNoTrackingAsync(icogramChatId);
            var mess = new StringBuilder(message);
            mess.Replace("[ChatName]", icogramChat.Title);
            await _telegramBotClient.SendTextMessageAsync(icogramChat.TelegramChatId, mess.ToString());
        }

        public async Task BanUserAsync(SuspiciousUser user)
        {
            try
            {
                await
                    _telegramBotClient.KickChatMemberAsync(user.Chat.TelegramChatId, user.TelegramUserId,
                        DateTime.UtcNow.AddDays(30));
                user.IsUserBanned = true;
                user.BannedDate = DateTime.UtcNow;
                await _suspiciousUserCrudService.UpdateAsync(user);
            }
            catch (Exception)
            {
            }
        }

        public async Task UnBanUserAsync(SuspiciousUser user)
        {
            try
            {
                await _telegramBotClient.UnbanChatMemberAsync(user.Chat.TelegramChatId, user.TelegramUserId);
                user.IsUserBanned = false;
                user.BannedDate = DateTime.UtcNow;
                await _suspiciousUserCrudService.UpdateAsync(user);
            }
            catch (Exception)
            {

            }
        }


        private async Task TryToExecuteCommand(Update update)
        {
            var command =
                _chat.Commands.FirstOrDefault(
                    c =>
                        $"/{c.CommandName}" == update.Message.Text ||
                        $"/{c.CommandName}@{IcogramBotSettings.Name}" == update.Message.Text);
            var check = AccessCheck(GlobalEnums.ModuleType.CommandModule);
            if (check)
            {
                if (!IsNullOrEmpty(command?.ActionMessage))
                    if (command.LastUsage.HasValue)
                    {
                        if (command.LastUsage.Value.AddSeconds(command.Chat.CommandTimeOut) <= DateTime.UtcNow)
                        {
                            await
                                _telegramBotClient.SendTextMessageAsync(_chat.TelegramChatId, command.ActionMessage,
                                    replyToMessageId: update.Message.MessageId);
                            command = await _commandCrudService.GetByIdAsync(command.Id);
                            command.LastUsage = DateTime.UtcNow;
                            await _commandCrudService.UpdateAsync(command);
                        }

                    }
                    else
                    {
                        await
                            _telegramBotClient.SendTextMessageAsync(_chat.TelegramChatId, command.ActionMessage,
                                replyToMessageId: update.Message.MessageId);
                        command = await _commandCrudService.GetByIdAsync(command.Id);
                        command.LastUsage = DateTime.UtcNow;
                        await _commandCrudService.UpdateAsync(command);
                    }
            }
        }

        private async Task ShowListCommandsAsync(Update update)
        {
            var check = AccessCheck(GlobalEnums.ModuleType.CommandModule);
            if (update.Message.Text == $"/commands@{IcogramBotSettings.Name}" || update.Message.Text == "/commands")
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("You can use these commands: \n");
                if (check)
                {
                    foreach (var command in _chat.Commands)
                    {
                        if (command.IsCommandShowInList)
                        {
                            stringBuilder.Append($"/{command.CommandName} - {command.CommandDescription} \n");
                        }
                    }
                    await _telegramBotClient.SendTextMessageAsync(_chat.TelegramChatId, stringBuilder.ToString());
                }
            }

        }

        private async Task LinkCheck(Update update)
        {
            var whiteLinks = await _whiteLinkCrudService.GetAllAsync();
            var settings = await _antiSpamSettingsCrudService.GetAllAsync();
            var setting = settings.FirstOrDefault(s => s.ChatId == _chat.Id);
            if (setting != null && setting.IsModuleIncluded)
            {

                whiteLinks = whiteLinks.Where(wl => wl.ChatId == _chat.Id).ToList();
                try
                {
                    var isNeedToDelete = false;

                    foreach (var entity in update.Message.Entities.Where(e => e.Type == MessageEntityType.Url))
                    {
                        var item = update.Message.Text.Substring(entity.Offset, entity.Length);
                        var link = "";
                        if (!item.Contains("http") && !item.Contains("https") && !item.Contains("www"))
                        {
                            link = "www." + item;
                        }
                        else
                        {
                            var url = item.StartsWith("www.") ? new Uri("http://" + item) : new Uri(item);
                            link = url.Host;
                        }
                        var isWhiteLink = false;
                        if (!IsNullOrEmpty(link) && link.Contains("."))
                        {
                            var words = link.Split('.');
                            if (words.Length > 1)
                            {
                                var word = words[words.Length - 2].ToLower();
                                var isAny = whiteLinks.Any(wl => wl.Link.ToLower() == word);
                                if (isAny && whiteLinks.Count > 0)
                                {
                                    isWhiteLink = true;
                                }
                            }
                        }
                            if (!isWhiteLink)
                            {
                                isNeedToDelete = true;
                            }
                    }

                    if (isNeedToDelete)
                    {
                        

                        var users = await _suspiciousUserCrudService.GetAllAsync();
                        var user = users.FirstOrDefault(u => u.TelegramUserId == update.Message.From.Id);
                        if (user == null)
                        {
                            user = new SuspiciousUser
                            {
                                FirstName = update.Message.From.FirstName ?? "",
                                LastName = update.Message.From.LastName ?? "",
                                UserName = update.Message.From.Username ?? "",
                                ChatId = _chat.Id,
                                NumberOfAttempts = 1,
                                TelegramUserId = update.Message.From.Id
                            };

                            await _suspiciousUserCrudService.CreateAsync(user);
                        }
                        else
                        {
                            user.NumberOfAttempts++;
                            await _suspiciousUserCrudService.UpdateAsync(user);
                        }

                        await _telegramBotClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                        var mess = new StringBuilder(setting.WarningMessage);
                        SetUserParams(ref mess, update.Message.From);
                        mess.Replace("[NumberOfAttempts]", user.NumberOfAttempts.ToString());
                        await
                            _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, mess.ToString());

                        if (setting.IsNeededToBanUser && setting.NumberOfAttempts <= user.NumberOfAttempts)
                        {
                            await BanUserAsync(user);
                        }

                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task NewUserCheck(Update update)
        {
            var check = AccessCheck(GlobalEnums.ModuleType.WelcomeMessageModule);
            if (update.Message.NewChatMember != null && check)
            {
                var welcomeMessage = new StringBuilder(_chat.WelcomeUserMessage);
                SetUserParams(ref welcomeMessage, update.Message.NewChatMember);
                SetChatParams(ref welcomeMessage, update);
                if (!IsNullOrEmpty(update.Message.NewChatMember.Username))
                {
                    welcomeMessage.Insert(0, $"@{update.Message.NewChatMember.Username} \n");
                }
                if (_chat.IsNeededToDeleteNewUserMessage)
                {
                    await _telegramBotClient.DeleteMessageAsync(_chat.TelegramChatId, update.Message.MessageId);
                }
                await _telegramBotClient.SendTextMessageAsync(_chat.TelegramChatId, welcomeMessage.ToString());
            }
        }

        private async Task LeaveUserCheck(Update update)
        {
            if (_chat.IsNeededToDeleteLeaveUserMessage)
            {
                await _telegramBotClient.DeleteMessageAsync(_chat.TelegramChatId, update.Message.MessageId);
            }
        }

        private bool AccessCheck(GlobalEnums.ModuleType type)
        {
            const bool result = false;
            if (!_chat.IsApproved) return result;
            if (!_chat.Company.End.HasValue) return result;
            if (_chat.Company.End.Value.Date < DateTime.UtcNow.Date) return result;
            switch (type)
            {
                case GlobalEnums.ModuleType.NotSet:
                    return true;
                case GlobalEnums.ModuleType.WelcomeMessageModule:
                    return _chat.Company.IsWelcomeMessageModuleActivated;
                case GlobalEnums.ModuleType.CommandModule:
                    return _chat.Company.IsCommandModuleActivated;
                case GlobalEnums.ModuleType.AntiSpamModule:
                    return _chat.Company.IsAntiSpamModuleActivated;
                case GlobalEnums.ModuleType.CustomMessageModule:
                    return _chat.Company.IsCustomMessageModuleActivated;
                default:
                    return result;
            }
        }

        private static void SetChatParams(ref StringBuilder message, Update update)
        {
            if (update.Message.Chat != null)
            {
                message.Replace("[ChatUserName]",
                    IsNullOrEmpty(update.Message.Chat.Username) ? "" : update.Message.Chat.Username);
                message.Replace("[ChatFirstName]",
                    IsNullOrEmpty(update.Message.Chat.FirstName) ? "" : update.Message.Chat.FirstName);
                message.Replace("[ChatLastName]",
                    IsNullOrEmpty(update.Message.Chat.LastName) ? "" : update.Message.Chat.LastName);
                message.Replace("[ChatTitle]",
                    IsNullOrEmpty(update.Message.Chat.Title) ? "" : update.Message.Chat.Title);
                message.Replace("[ChatInviteLink]",
                    IsNullOrEmpty(update.Message.Chat.InviteLink) ? "" : update.Message.Chat.InviteLink);
                message.Replace("[ChatDescription]",
                    IsNullOrEmpty(update.Message.Chat.Description) ? "" : update.Message.Chat.Description);
            }
        }

        private static void SetUserParams(ref StringBuilder message, User user)
        {
            message.Replace("[FirstName]",
                    IsNullOrEmpty(user.FirstName)
                        ? ""
                        : user.FirstName);
            message.Replace("[LastName]",
                IsNullOrEmpty(user.LastName)
                    ? ""
                    : user.LastName);
            message.Replace("[UserName]",
                IsNullOrEmpty(user.Username)
                    ? ""
                    : user.Username);
        }
    }
}