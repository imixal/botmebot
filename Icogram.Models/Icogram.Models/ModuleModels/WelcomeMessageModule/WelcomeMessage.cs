using Icogram.Models.Abstract;
using Icogram.Models.BotModels;

namespace Icogram.Models.ModuleModels.WelcomeMessageModule
{
    public class WelcomeMessage : Entity
    {
        public string Message { get; set; }

        public CustomerBot Bot { get; set; }

        public int BotId { get; set; }
    }
}