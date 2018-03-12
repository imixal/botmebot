using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler.MessageBotHandler
{
    public interface ILinkChecker
    {
        Task MessageCheck(Update update, Message message, Chat chat, ITelegramBotClient telegramBotClient);

        Task CaptionCheck(Update update, Chat chat, ITelegramBotClient telegramBotClient);
    }
}