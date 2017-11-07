using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icogram.Service.ChatStatistic
{
    public interface IChatStatisticService
    {
        Task<List<Models.ModuleModels.StatisticsModule.ChatStatistic>> GetChatStatistic(int id);
    }
}