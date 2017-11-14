using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Enums;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Service;
using Icogram.Telegram.Bot.Bot;
using Icogram.Telegram.BotHandler.CommandBotHandler;
using Icogram.Telegram.BotHandler.StatisticBotHandler;
using Icogram.Telegram.BotHandler.UserBotHandler;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.String;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler
{
    public partial class BotHandler : IBotHandler
    {
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly ICrudService<AntiSpamSetting> _antiSpamSettingsCrudService;
        private readonly ICrudService<WhiteLink> _whiteLinkCrudService;
        private readonly ICrudService<SuspiciousUser> _suspiciousUserCrudService;
        private readonly TelegramBotClient _telegramBotClient;
        private readonly ICommandHandler _commandHandler;
        private readonly IStatisticHandler _statisticHandler;
        private readonly IUserHandler _userHandler;
        private Chat _chat;
        private readonly Logger _logger;


        public BotHandler(ICrudService<Chat> chatCrudService, ICrudService<WhiteLink> whiteLinkCrudService,
            ICrudService<AntiSpamSetting> antiSpamSettingsCrudService,
            ICrudService<SuspiciousUser> suspiciousUserCrudService,
            Logger logger, ICommandHandler commandHandler, IStatisticHandler statisticHandler, IUserHandler userHandler)
        {
            _chatCrudService = chatCrudService;
            _whiteLinkCrudService = whiteLinkCrudService;
            _antiSpamSettingsCrudService = antiSpamSettingsCrudService;
            _suspiciousUserCrudService = suspiciousUserCrudService;
            _logger = logger;
            _commandHandler = commandHandler;
            _statisticHandler = statisticHandler;
            _userHandler = userHandler;
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
                            try
                            {
                                await _statisticHandler.AddNewMessageAsync(_chat.Id);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddMessageError}");
                            }
                            try
                            {
                                await _statisticHandler.AddSymbolsInMessageAsync(_chat.Id, update.Message.Text.Length);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddSymbolsInMessageError}");
                            }
                            if (update.Message.Entities != null)
                            {
                                if (update.Message.Entities.Any(e => e.Type == MessageEntityType.BotCommand))
                                {
                                    try
                                    {
                                        await _commandHandler.ShowListCommandsAsync(update, chat);
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.Error(e, $"{Errors.CommandErorr}: {Errors.ShowListCommandsError}");
                                    }
                                }
                                if (update.Message.Entities.Any(e => e.Type == MessageEntityType.Url))
                                {
                                    await LinkCheck(update);
                                }
                            }
                            try
                            {
                                await _commandHandler.ExecuteCommandAsync(update, _chat);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, $"{Errors.CommandErorr}: {Errors.ExecuteCommandError}");
                            }
                        }
                        if (update.Message.LeftChatMember != null)
                        {
                            await _userHandler.UserLeavedAsync(update, _chat, _telegramBotClient);
                            try
                            {
                                await _statisticHandler.AddLeavedUserAsync(chat.Id);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddLeavedUserError}");
                            }
                        }

                        if (update.Message.NewChatMembers?.Length > 0)
                        {
                            await _userHandler.UsersAddAsync(update, _chat, _telegramBotClient);
                            try
                            {
                                await _statisticHandler.AddNewUsersAsync(_chat.Id, update.Message.NewChatMembers.Length);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddNewUsersError}");
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Message Handler");

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
            catch (Exception e)
            {
                _logger.Error(e, "Get Approved Chat");
            }
            return null;
        }

        public async Task UpdateChatFieldsAsync(int icogramChatId)
        {
            try
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
            catch (Exception e)
            {
                _logger.Error(e, "Update chat fields");
            }
        }

        public async Task LeaveChatAsync(int icogramChatId)
        {
            try
            {
                var icogramChat = await _chatCrudService.GetByIdAsNoTrackingAsync(icogramChatId);
                await _telegramBotClient.LeaveChatAsync(icogramChat.TelegramChatId);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Leave chat");
            }
        }

        public async Task SendMessageAsync(int icogramChatId, string message)
        {
            try
            {
                var icogramChat = await _chatCrudService.GetByIdAsNoTrackingAsync(icogramChatId);
                var mess = new StringBuilder(message);
                mess.Replace("[ChatName]", icogramChat.Title);
                await _telegramBotClient.SendTextMessageAsync(icogramChat.TelegramChatId, mess.ToString());
            }
            catch (Exception e)
            {
                _logger.Error(e, "Send message");
            }
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
                try
                {
                    await _statisticHandler.AddBannedUserAsync(_chat.Id);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddBannedUserError}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Ban user");
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
            catch (Exception e)
            {
                _logger.Error(e, "Unban user");
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
                        var user =
                            users.FirstOrDefault(u => u.TelegramUserId == update.Message.From.Id && u.ChatId == _chat.Id);
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
                            var oldUser = await _suspiciousUserCrudService.GetByIdAsync(user.Id);
                            await _suspiciousUserCrudService.UpdateAsync(oldUser);
                        }

                        await _telegramBotClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                        try
                        {
                            await _statisticHandler.AddDeletedMessageAsync(_chat.Id);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddDeletedMessageError}");
                        }
                        var mess = new StringBuilder(setting.WarningMessage);
                        ParamsSetter.SetUserParams(ref mess, update.Message.From);
                        mess.Replace("[NumberOfAttempts]", user.NumberOfAttempts.ToString());
                        await
                            _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, mess.ToString());

                        if (setting.IsNeededToBanUser && setting.NumberOfAttempts <= user.NumberOfAttempts)
                        {
                            await BanUserAsync(user);
                        }

                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Link check");
                }
            }
        }
    }
}