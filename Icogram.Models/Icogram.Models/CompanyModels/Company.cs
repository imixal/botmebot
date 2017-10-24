using System.Collections.Generic;
using Icogram.Enums;
using Icogram.Models.Abstract;
using Icogram.Models.BotModels;
using Icogram.Models.UserModels;

namespace Icogram.Models.CompanyModels
{
    public class Company : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public GlobalEnums.CompanyType Type { get; set; }

        public CompanyTarif Tarif { get; set; }

        public int? TarifId { get; set; }

        public List<CustomerBot> TelegramBots { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public List<CompanyChat> Chats { get; set; }
    }
}