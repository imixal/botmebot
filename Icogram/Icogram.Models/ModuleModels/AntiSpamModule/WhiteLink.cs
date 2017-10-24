using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.AntiSpamModule
{
    public class WhiteLink : Entity
    {
        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public string Link { get; set; }
    }
}