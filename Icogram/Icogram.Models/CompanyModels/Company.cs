using System;
using System.Collections.Generic;
using Icogram.Enums;
using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;
using Icogram.Models.Payments;
using Icogram.Models.UserModels;

namespace Icogram.Models.CompanyModels
{
    public class Company : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public GlobalEnums.CompanyType Type { get; set; }

        public bool IsWelcomeMessageModuleActivated { get; set; }

        public bool IsCommandModuleActivated { get; set; }

        public bool IsAntiSpamModuleActivated { get; set; }

        public bool IsCustomMessageModuleActivated { get; set; }

        public DateTime? End { get; set; }

        public double Price { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public List<Chat> Chats { get; set; }

        public List<Payment> Payments { get; set; }
    }
}