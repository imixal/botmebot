using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler.UserBotHandler
{
    public interface IUserHandler
    {
        Task UsersAddAsync(Update update, Chat chat, ITelegramBotClient telegramBotClient);

        Task UserLeavedAsync(Update update, Chat chat, ITelegramBotClient telegramBotClient);
    }
}