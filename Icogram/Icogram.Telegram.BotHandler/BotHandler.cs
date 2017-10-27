using System;
using System.Linq;
using System.Threading.Tasks;
using Service;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Telegram.Bot.Bot;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler
{
    public class BotHandler : IBotHandler
    {
        private readonly ICrudService<Chat> _chatCrudService;
        private readonly ICrudService<ChatCommand> _chatCommandCrudService;
        private readonly TelegramBotClient _telegramBotClient;

        public BotHandler(ICrudService<Chat> chatCrudService, ICrudService<ChatCommand> chatCommandCrudService)
        {
            _chatCrudService = chatCrudService;
            _chatCommandCrudService = chatCommandCrudService;
            _telegramBotClient = IcogramBot.GetClient();
        }


        public async Task<bool> IsChatApprovedAsync(long telegramChatId)
        {
            var chats = await _chatCrudService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.TelegramChatId == telegramChatId);
            if (chat == null)
            {
                var telegramChat = await _telegramBotClient.GetChatAsync(telegramChatId);
                var icogramChat = new Chat {IsApproved = false, TelegramChatId = telegramChatId, Title = telegramChat.Title, Type = telegramChat.Type.ToString()};
                await _chatCrudService.CreateAsync(icogramChat);

                return false;
            }
            else
            {
                return chat.IsApproved;
            }
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
            await _telegramBotClient.SendTextMessageAsync(icogramChat.TelegramChatId, message);
        }

        public async Task ExecuteCommandAsync(Update update)
        {
            var chatCommands = await _chatCommandCrudService.GetAllAsync();
            chatCommands = chatCommands.Where(cc => cc.Chat.TelegramChatId == update.Message.Chat.Id).ToList();

            var command =
                chatCommands.FirstOrDefault(cc => $"/{cc.Command.CommandName}@{IcogramBotSettings.Name}" == update.Message.Text);

            if (command != null)
            {
                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, command.Message);
            }
        }
    }
}