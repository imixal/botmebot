using System.Collections.Generic;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.Command
{
    public class MyCommandsPageViewModel : LayoutViewModel
    {
        public List<Models.ChatModels.Chat> Chats { get; set; }
    }
}