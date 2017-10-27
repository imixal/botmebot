using System.Collections.Generic;
using Icogram.Models.CompanyModels;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.Chat
{
    public class ChatsPageViewModel : LayoutViewModel
    {
        public List<Models.ChatModels.Chat> Chats { get; set; }

        public List<Company> Companies { get; set; }
    }
}