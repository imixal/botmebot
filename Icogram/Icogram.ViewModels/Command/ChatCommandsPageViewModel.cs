using System.Collections.Generic;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.Command
{
    public class ChatCommandsPageViewModel : LayoutViewModel
    {
        public List<Models.ModuleModels.CommandModule.Command> Commands { get; set; }

        public List<ChatCommand> ChatCommands { get; set; }

        public List<Models.ChatModels.Chat> Chats { get; set; }
    }
}