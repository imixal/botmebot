using System.Threading.Tasks;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler.CommandBotHandler
{
    public interface ICommandHandler
    {
        Task ExecuteCommandAsync(Update update, Chat chat);

        Task ShowListCommandsAsync(Update update, Chat chat);
    }
}