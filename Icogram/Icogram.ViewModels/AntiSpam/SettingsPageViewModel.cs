using System.Collections.Generic;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.AntiSpam
{
    public class SettingsPageViewModel : LayoutViewModel
    {
        public List<Models.ChatModels.Chat> Chats { get; set; }

        public List<AntiSpamSetting> Settings { get; set; }
    }
}