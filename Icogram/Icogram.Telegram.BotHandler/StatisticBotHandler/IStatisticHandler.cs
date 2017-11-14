using System.Threading.Tasks;

namespace Icogram.Telegram.BotHandler.StatisticBotHandler
{
    public interface IStatisticHandler
    {
        Task AddCommandAsync(int chatId);

        Task AddNewMessageAsync(int chatId);

        Task AddSymbolsInMessageAsync(int chatId, int length);

        Task AddBannedUserAsync(int chatId);

        Task AddDeletedMessageAsync(int chatId);

        Task AddLeavedUserAsync(int chatId);

        Task AddNewUsersAsync(int chatId, int count);
    }
}