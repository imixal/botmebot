using System;
using Icogram.Models.Abstract;

namespace Icogram.Models.CompanyModels
{
    public class CompanyTarif : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsWelcomeMessageModuleActivated { get; set; }

        public bool IsCommandModuleActivated { get; set; }

        public bool IsNotificationModuleActivated { get; set; }

        public bool IsCustomMessageModuleActivated { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public double Price { get; set; }
    }
}