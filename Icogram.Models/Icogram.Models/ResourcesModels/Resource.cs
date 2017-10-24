using Icogram.Models.Abstract;

namespace Icogram.Models.ResourcesModels
{
    public class Resource : Entity
    {
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public string EnglishValue { get; set; }

        public string RussianValue { get; set; }
    }
}