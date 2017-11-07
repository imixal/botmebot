using System.Collections.Generic;
using System.Threading.Tasks;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.ChatStatistic
{
    public class MyStatisticPageViewModel : LayoutViewModel
    {
        public List<Models.ModuleModels.StatisticsModule.ChatStatistic> ChatStats { get; set; }

        public List<Models.ChatModels.Chat> Chats { get; set; }

        public bool IsChatsEmpty { get; set; }
    }
}