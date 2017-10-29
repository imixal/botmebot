using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Enums;
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
        private readonly TelegramBotClient _telegramBotClient;
        private Chat _chat;

        public BotHandler(ICrudService<Chat> chatCrudService, ICrudService<Command> commandCrudService)
        {
            _chatCrudService = chatCrudService;
            _commandCrudService = commandCrudService;
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
            var icogramChat = await _chatCrudService.GetByIdAsNoTrackingAsync(icogramChatId);
            await _telegramBotClient.LeaveChatAsync(icogramChat.TelegramChatId);
        }

        public async Task SendMessageAsync(int icogramChatId, string message)
        {
            var icogramChat = await _chatCrudService.GetByIdAsNoTrackingAsync(icogramChatId);
            var mess = new StringBuilder(message);
            mess.Replace("[ChatName]", icogramChat.Title);
            await _telegramBotClient.SendTextMessageAsync(icogramChat.TelegramChatId, mess.ToString());
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
            try
            {
                await _telegramBotClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                await
                    _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                        $"Please, don't send a link, {update.Message.From.FirstName} {update.Message.From.LastName}");
            }
            catch (Exception)
            {
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