using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Enums;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler.UserBotHandler
{
    public class UserHandler : IUserHandler
    {
        private readonly Logger _logger;



        public UserHandler(Logger logger)
        {
            _logger = logger;
        }


        public async Task UsersAddAsync(Update update, Chat chat, ITelegramBotClient telegramBotClient)
        {
            if (!string.IsNullOrEmpty(chat.WelcomeUserMessage))
            {
                var welcomeMessage = new StringBuilder(chat.WelcomeUserMessage);
                if (update.Message.NewChatMembers.Length == 1)
                {
                    try
                    {
                        ParamsSetter.SetUserParams(ref welcomeMessage, update.Message.NewChatMembers.First());
                        ParamsSetter.SetChatParams(ref welcomeMessage, update);
                        if (!string.IsNullOrEmpty(update.Message.NewChatMembers.First().Username))
                        {
                            welcomeMessage.Insert(0, $"@{update.Message.NewChatMembers.First().Username} \n");
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.StackTrace, $"{Errors.UserError}: {Errors.WelcomeMessageSetParams}");
                    }

                    try
                    {
                        await telegramBotClient.SendTextMessageAsync(chat.TelegramChatId, welcomeMessage.ToString());
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.StackTrace, $"{Errors.UserError}: {Errors.SendWelcomeMessage}");
                    }
                }
                else
                {
                    try
                    {
                        ParamsSetter.SetChatParams(ref welcomeMessage, update);
                        ParamsSetter.SetUserParams(ref welcomeMessage, new User());
                        foreach (var newUser in update.Message.NewChatMembers)
                        {
                            if (!string.IsNullOrEmpty(newUser.Username))
                            {
                                welcomeMessage.Insert(0, $"@{newUser.Username} \n");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.StackTrace, $"{Errors.UserError}: {Errors.WelcomeMessageSetParams}");
                    }
                    try
                    {
                        await telegramBotClient.SendTextMessageAsync(chat.TelegramChatId, welcomeMessage.ToString());
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.StackTrace, $"{Errors.UserError}: {Errors.SendWelcomeMessage}");
                    }
                }
            }
            if (chat.IsNeededToDeleteNewUserMessage)
            {
                await telegramBotClient.DeleteMessageAsync(chat.TelegramChatId, update.Message.MessageId);
            }
        }

        public async Task UserLeavedAsync(Update update, Chat chat, ITelegramBotClient telegramBotClient)
        {
            if (chat.IsNeededToDeleteLeaveUserMessage)
            {
                try
                {
                    await telegramBotClient.DeleteMessageAsync(chat.TelegramChatId, update.Message.MessageId);
                }
                catch (Exception e)
                {
                    _logger.Error(e.StackTrace, $"{Errors.UserError}: {Errors.LeavedUserError}");
                }
            }
        }
    }
}