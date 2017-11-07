using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.Models.ModuleModels.StatisticsModule;

namespace Icogram.DataAccessLayer.Interfaces
{
    public interface IChatStatisticRepository : IRepository<ChatStatistic>
    {
        Task AddNewMessage(int chatId);

        Task AddDeletedMessage(int chatId);

        Task AddNewUser(int chatId);

        Task AddLeavedUser(int chatId);

        Task AddBannedUser(int chatId);

        Task AddCommand(int chatId);

        Task AddSymbolsInMessage(int chatId, int value);

        Task<List<ChatStatistic>> GetChatStatistic(int chatId);
    }
}