using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.CommandModule
{
    public class ChatCommand : Entity
    {
        public Command Command { get; set; }

        public int CommandId { get; set; }

        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public string Message { get; set; }

        public int NumberOfUsage { get; set; }
    }
}