using System.Collections.Generic;
using Icogram.Models.UserModels;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.User
{
    public class UserListViewModel : LayoutViewModel
    {
        public List<Models.CompanyModels.Company> Companies { get; set; }

        public List<ApplicationUser> Users { get; set; } 
    }
}