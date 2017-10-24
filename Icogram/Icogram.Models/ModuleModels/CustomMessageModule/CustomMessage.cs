using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.CustomMessageModule
{
    public class CustomMessage: Entity
    {
        public string Message { get; set; }

        public Chat Chat { get; set; }

        public int ChatId { get; set; }
    }
}