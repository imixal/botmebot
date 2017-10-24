using Icogram.Models.Abstract;

namespace Icogram.Models.ModuleModels.CommandModule
{
    public class Command : Entity
    {
        public string Name { get; set; }

        public string CommandName { get; set; }

        public string CommandDescription { get; set; }
    }
}