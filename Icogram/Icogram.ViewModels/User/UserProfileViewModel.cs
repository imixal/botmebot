using Icogram.Models.CompanyModels;

namespace Icogram.ViewModels.User
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Company Company { get; set; }
    }
}