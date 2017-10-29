using System;
using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.AntiSpamModule
{
    public class SuspiciousUser : Entity
    {
        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public int TelegramUserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int NumberOfAttempts { get; set; }

        public bool IsUserBanned { get; set; }

        public DateTime? BannedDate { get; set; }
    }
}