using Icogram.Models.Abstract;
using Icogram.Models.CompanyModels;
using Icogram.Models.ModuleModels.WelcomeMessageModule;

namespace Icogram.Models.BotModels
{
    public class CustomerBot : Entity
    {
        public int BotId { get; set; }

        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public WelcomeMessage WelcomeMessage { get; set; }

        public int? WelcomeMessageId { get; set; }
    }
}