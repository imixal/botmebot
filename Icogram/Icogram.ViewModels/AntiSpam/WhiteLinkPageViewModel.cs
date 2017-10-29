using System.Collections.Generic;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.AntiSpam
{
    public class WhiteLinkPageViewModel : LayoutViewModel
    {
        public List<WhiteLink> WhiteLinks { get; set; }

        public List<Models.ChatModels.Chat> Chats { get; set; }
    }
}