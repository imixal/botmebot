using System.Threading.Tasks;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler
{
    public interface IBotHandler
    {
        Task<Chat> GetApprovedChatAsync(long telegramChatId);

        Task<Chat> GetUnApprovedChatAsync(long telegramChatId);

        Task UpdateChatFieldsAsync(int icogramChatId);

        Task LeaveChatAsync(int icogramChatId);

        Task SendMessageAsync(int icogramChatId, string message);

        Task MessageHandler(Update update, Chat chat);

        Task UnApprovedChatHandler(Update update, Chat chat);

        Task UnBanUserAsync(SuspiciousUser user);

        Task BanUserAsync(SuspiciousUser user);
    }
}