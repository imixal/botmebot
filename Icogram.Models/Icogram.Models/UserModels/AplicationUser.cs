using System;
using Icogram.Models.Abstract;
using Icogram.Models.CompanyModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.Models.UserModels
{
    public class AplicationUser : IdentityUser, IEntity<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public DateTime DateCreation { get; }

        public AplicationUser()
        {
            DateCreation = DateTime.UtcNow;
        }
    }
}