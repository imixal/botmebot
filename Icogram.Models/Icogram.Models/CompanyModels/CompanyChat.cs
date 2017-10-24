using Icogram.Models.Abstract;

namespace Icogram.Models.CompanyModels
{
    public class CompanyChat : Entity
    {
        public string ChatId { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public Company Company { get; set; }

        public int? CompanyId { get; set; }
    }
}