using System.Collections.Generic;
using System.Threading.Tasks;
using Icogram.DataAccessLayer.Interfaces;

namespace Icogram.Service.ChatStatistic
{
    public class ChatStatisticService : IChatStatisticService
    {
        private readonly IChatStatisticRepository _chatStatisticRepository;


        public ChatStatisticService(IChatStatisticRepository chatStatisticRepository)
        {
            _chatStatisticRepository = chatStatisticRepository;
        }


        public async Task<List<Models.ModuleModels.StatisticsModule.ChatStatistic>> GetChatStatistic(int id)
        {
            return await _chatStatisticRepository.GetChatStatistic(id);
        }
    }
}