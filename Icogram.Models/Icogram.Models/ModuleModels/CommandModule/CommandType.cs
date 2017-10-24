using Icogram.Models.Abstract;

namespace Icogram.Models.ModuleModels.CommandModule
{
    public class CommandType: Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Rule { get; set; }
    }
}