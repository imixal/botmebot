using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Icogram.Enums;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Service;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Telegram.Bot.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
            _chat = chat;
            if (update.Type == UpdateType.MessageUpdate)
            {
                if (update.Message != null)
                {
                    if (!string.IsNullOrEmpty(update.Message.Text))
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
                    await NewUserCheck(update);
                }

            }
        }

        public async Task<Chat> GetApprovedChatAsync(long telegramChatId)
        {
            var chats = await _chatCrudService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.TelegramChatId == telegramChatId);
            if (chat != null) return chat.IsApproved ? chat : null;
            var telegramChat = await _telegramBotClient.GetChatAsync(telegramChatId);
            var icogramChat = new Chat
            {
                IsApproved = false,
                TelegramChatId = telegramChatId,
                Title = telegramChat.Title,
                Type = telegramChat.Type.ToString()
            };
            await _chatCrudService.CreateAsync(icogramChat);

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
                await _telegramBotClient.KickChatMemberAsync(user.Chat.TelegramChatId, user.TelegramUserId, DateTime.UtcNow.AddDays(30));
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
            var command = _chat.Commands.FirstOrDefault(c => $"!{c.CommandName}" == update.Message.Text);
            var check = AccessCheck(GlobalEnums.ModuleType.CommandModule);
            if (check)
            {
                if (command != null && string.IsNullOrEmpty(command.ActionMessage))
                    await _telegramBotClient.SendTextMessageAsync(_chat.TelegramChatId, command.ActionMessage);
            }
        }

        private async Task ShowListCommandsAsync(Update update)
        {
            var check = AccessCheck(GlobalEnums.ModuleType.CommandModule);
            if (update.Message.Text == $"/info@{IcogramBotSettings.Name}" || update.Message.Text == "/info" || update.Message.Text == "/help" || update.Message.Text == "/man")
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("You can use this command: \n");
                if (check)
                {
                    foreach (var command in _chat.Commands)
                    {
                        if (command.IsCommandShowInList)
                        {
                            stringBuilder.Append($"!{command.CommandName} \n");
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

                    foreach (
                        Match item in
                        Regex.Matches(update.Message.Text, @"\b(?:https?://|www\.)[^ \f\n\r\t\v\]]+\b",
                            RegexOptions.Compiled | RegexOptions.IgnoreCase))
                    {
                        var url = item.Value.StartsWith("www.") ? new Uri("http://" + item.Value) : new Uri(item.Value);
                        var link = url.GetLeftPart(UriPartial.Path).Remove(url.GetLeftPart(UriPartial.Path).Length - 1);
                        var isWhiteLink = whiteLinks.Any(
                            wl => wl.Link == link ||
                                  $"http://{wl.Link}" == link ||
                                  $"https://{wl.Link}" == link ||
                                  wl.Link == link.Replace("www.", "") ||
                                  wl.Link == link.Replace("http://", "") ||
                                  wl.Link == link);
                        if (setting.IsInvertMode)
                        {
                            if (isWhiteLink)
                            {
                                isNeedToDelete = true;
                            }
                        }
                        else
                        {
                            if (!isWhiteLink)
                            {
                                isNeedToDelete = true;
                            }
                        }
                    }

                    if (isNeedToDelete)
                    {
                        await _telegramBotClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                        await
                            _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                setting.WarningMessage.Replace("[UserName]",
                                    $"{update.Message.From.FirstName} {update.Message.From.LastName}"));

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
                welcomeMessage.Replace("[UserName]", $"{update.Message.NewChatMember.FirstName} {update.Message.NewChatMember.LastName}");
                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, welcomeMessage.ToString());
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
    }
}