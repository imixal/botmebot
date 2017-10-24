using Icogram.Models.Abstract;

namespace Icogram.Models.EmailModels
{
    public class EmailVariable : Entity
    {
        public string Variable { get; set; }

        public string Description { get; set; }
    }
}