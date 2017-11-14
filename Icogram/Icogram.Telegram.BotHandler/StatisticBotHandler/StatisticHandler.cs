using System.Threading.Tasks;
using Icogram.DataAccessLayer.Interfaces;

namespace Icogram.Telegram.BotHandler.StatisticBotHandler
{
    public class StatisticHandler : IStatisticHandler
    {
        private readonly IChatStatisticRepository _chatStatisticRepository;


        public StatisticHandler(IChatStatisticRepository chatStatisticRepository)
        {
            _chatStatisticRepository = chatStatisticRepository;
        }


        public async Task AddCommandAsync(int chatId)
        {
            await _chatStatisticRepository.AddCommand(chatId);
        }

        public async Task AddNewMessageAsync(int chatId)
        {
            await _chatStatisticRepository.AddNewMessage(chatId);
        }

        public async Task AddSymbolsInMessageAsync(int chatId, int length)
        {
            await _chatStatisticRepository.AddSymbolsInMessage(chatId, length);
        }

        public async Task AddBannedUserAsync(int chatId)
        {
            await _chatStatisticRepository.AddBannedUser(chatId);
        }

        public async Task AddDeletedMessageAsync(int chatId)
        {
            await _chatStatisticRepository.AddDeletedMessage(chatId);
        }

        public async Task AddLeavedUserAsync(int chatId)
        {
            await _chatStatisticRepository.AddLeavedUser(chatId);
        }

        public async Task AddNewUsersAsync(int chatId, int count)
        {
            await _chatStatisticRepository.AddNewUsers(chatId, count);
        }
    }
}