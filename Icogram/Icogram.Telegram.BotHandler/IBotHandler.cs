using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Icogram.Telegram.BotHandler
{
    public interface IBotHandler
    {
        Task<bool> IsChatApprovedAsync(long telegramChatId);

        Task UpdateChatFieldsAsync(int icogramChatId);

        Task LeaveChatAsync(int icogramChatId);

        Task SendMessageAsync(int icogramChatId, string message);

        Task MessageHandler(Update update);
    }
}