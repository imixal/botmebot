using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Service;
using Icogram.Telegram.Bot.Bot;
using Icogram.Telegram.BotHandler.CommandBotHandler;
using Icogram.Telegram.BotHandler.MessageBotHandler;
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
        private readonly ICrudService<SuspiciousUser> _suspiciousUserCrudService;
        private readonly TelegramBotClient _telegramBotClient;
        private readonly ICommandHandler _commandHandler;
        private readonly IStatisticHandler _statisticHandler;
        private readonly IUserHandler _userHandler;
        private Chat _chat;
        private readonly Logger _logger;
        private readonly ILinkChecker _linkChecker;


        public BotHandler(ICrudService<Chat> chatCrudService,
            ICrudService<SuspiciousUser> suspiciousUserCrudService,
            Logger logger, ICommandHandler commandHandler, IStatisticHandler statisticHandler, IUserHandler userHandler,
            ILinkChecker linkChecker)
        {
            _chatCrudService = chatCrudService;
            _suspiciousUserCrudService = suspiciousUserCrudService;
            _logger = logger;
            _commandHandler = commandHandler;
            _statisticHandler = statisticHandler;
            _userHandler = userHandler;
            _linkChecker = linkChecker;
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
                        if (_chat.IsNeededToDeleteAllMessages)
                        {
                            try
                            {
                                var chatMember = await _telegramBotClient.GetChatMemberAsync(_chat.TelegramChatId,update.Message.From.Id);
                                if (chatMember.Status != ChatMemberStatus.Administrator &&
                                    chatMember.Status != ChatMemberStatus.Creator)
                                {
                                    await _telegramBotClient.DeleteMessageAsync(_chat.TelegramChatId, update.Message.MessageId);
                                    return;
                                }
                            }
                            catch (Exception)
                            {
                                
                            }
                        }
                        if (!IsNullOrEmpty(update.Message.Text))
                        {
                            try
                            {
                                await _statisticHandler.AddNewMessageAsync(_chat.Id);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e.StackTrace, $"{Errors.StatisticErorr}: {Errors.AddMessageError}");
                            }
                            try
                            {
                                await _statisticHandler.AddSymbolsInMessageAsync(_chat.Id, update.Message.Text.Length);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e.StackTrace,
                                    $"{Errors.StatisticErorr}: {Errors.AddSymbolsInMessageError}");
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
                                        _logger.Error(e.StackTrace,
                                            $"{Errors.CommandErorr}: {Errors.ShowListCommandsError}");
                                    }
                                }
                                if (update.Message.Entities.Any(e => e.Type == MessageEntityType.Url))
                                {
                                    if (!IsNullOrEmpty(update.Message.Text))
                                    {
                                        await _linkChecker.MessageCheck(update, update.Message, _chat, _telegramBotClient);
                                    }
                                }
                            }
                            try
                            {
                                await _commandHandler.ExecuteCommandAsync(update, _chat);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e.StackTrace, $"{Errors.CommandErorr}: {Errors.ExecuteCommandError}");
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
                                _logger.Error(e.StackTrace, $"{Errors.StatisticErorr}: {Errors.AddLeavedUserError}");
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
                                _logger.Error(e.StackTrace, $"{Errors.StatisticErorr}: {Errors.AddNewUsersError}");
                            }
                        }
                        if (update.Message.Entities != null &&
                            update.Message.Entities.Any(e => e.Type == MessageEntityType.Url))
                        {
                            if (!IsNullOrEmpty(update.Message.Caption))
                            {
                                await _linkChecker.CaptionCheck(update, _chat, _telegramBotClient);
                            }
                        }

                        if(update.EditedMessage != null)
                        {
                            if (!IsNullOrEmpty(update.EditedMessage.Text))
                            {
                                await _linkChecker.MessageCheck(update, update.EditedMessage, _chat, _telegramBotClient);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _logger.Error(e.StackTrace, "Message Handler");

            }
        }

        public async Task UnApprovedChatHandler(Update update, Chat chat)
        {
            try
            {
                _chat = chat;
                if (update.Type == UpdateType.MessageUpdate)
                {
                    if (update.Message?.NewChatMembers?.Length > 0)
                    {
                        await _userHandler.UsersAddAsync(update, _chat, _telegramBotClient);
                    }
                    if (!IsNullOrEmpty(update.Message?.Text))
                    {
                        if (update.Message.Entities.Any(e => e.Type == MessageEntityType.BotCommand))
                        {
                            try
                            {
                                await _commandHandler.ShowListCommandsAsync(update, chat);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e.StackTrace, $"{Errors.CommandErorr}: {Errors.ShowListCommandsError}");
                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                
                _logger.Error(e.StackTrace, "Can't handle UnApprovedChat");
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
                _logger.Error(e.StackTrace, "Get Approved Chat");
            }
            return null;
        }

        public async Task<Chat> GetUnApprovedChatAsync(long telegramChatId)
        {
            var chats = await _chatCrudService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.TelegramChatId == telegramChatId);

            return chat;
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
                _logger.Error(e.StackTrace, "Update chat fields");
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
                _logger.Error(e.StackTrace, "Leave chat");
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
                _logger.Error(e.StackTrace, "Send message");
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
                    _logger.Error(e.StackTrace, $"{Errors.StatisticErorr}: {Errors.AddBannedUserError}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.StackTrace, "Ban user");
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
                _logger.Error(e.StackTrace, "Unban user");
            }
        }
    }
}