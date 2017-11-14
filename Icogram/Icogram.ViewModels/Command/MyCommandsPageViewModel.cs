using System.Collections.Generic;
using Icogram.ViewModels.Layout;
using Icogram.Enums;

namespace Icogram.ViewModels.Command
{
    public class MyCommandsPageViewModel : LayoutViewModel
    {
        public List<Models.ChatModels.Chat> Chats { get; set; }

        public List<GlobalEnums.CommandType> Types { get; set; }
    }
}