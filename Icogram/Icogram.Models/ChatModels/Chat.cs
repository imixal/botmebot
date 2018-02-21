using System.Collections.Generic;
using Icogram.Models.Abstract;
using Icogram.Models.CompanyModels;
using Icogram.Models.ModuleModels.CommandModule;

namespace Icogram.Models.ChatModels
{
    public class Chat : Entity
    {
        public long TelegramChatId { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public Company Company { get; set; }

        public int? CompanyId { get; set; }

        public bool IsApproved { get; set; }

        public string WelcomeUserMessage { get; set; }

        public int CommandTimeOut { get; set; }

        public bool IsNeededToDeleteNewUserMessage { get; set; }

        public bool IsNeededToDeleteAllMessages { get; set; }

        public bool IsNeededToDeleteLeaveUserMessage { get; set; }

        public List<Command> Commands { get; set; }
    }
}