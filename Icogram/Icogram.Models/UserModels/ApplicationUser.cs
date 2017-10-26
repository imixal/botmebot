using System;
using Icogram.Models.Abstract;
using Icogram.Models.CompanyModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.Models.UserModels
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Company Company { get; set; }

        public int? CompanyId { get; set; }

        public DateTime DateCreation { get; }

        public ApplicationUser()
        {
            DateCreation = DateTime.UtcNow;
        }
    }
}