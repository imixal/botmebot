using Icogram.Models.Abstract;
using Icogram.Models.BotModels;

namespace Icogram.Models.ModuleModels.CustomMessageModule
{
    public class CustomMessage: Entity
    {
        public string Message { get; set; }

        public CustomerBot Bot { get; set; }

        public int BotId { get; set; }
    }
}