using Icogram.Models.Abstract;
using Icogram.Models.BotModels;

namespace Icogram.Models.ModuleModels.CommandModule
{
    public class Command : Entity
    {
        public string Name { get; set; }

        public string CommandName { get; set; }

        public string CommandDescription { get; set; }

        public CommandType Type { get; set; }

        public int TypeId { get; set; }

        public CustomerBot Bot { get; set; }

        public int BotId { get; set; }
    }
}