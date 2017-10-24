using Icogram.Models.Abstract;
using Icogram.Models.CompanyModels;

namespace Icogram.Models.ChatModels
{
    public class Chat : Entity
    {
        public string TelegramChatId { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public Company Company { get; set; }

        public int? CompanyId { get; set; }

        public bool IsApproved { get; set; }

        public string WelcomeUserMessage { get; set; }
    }
}