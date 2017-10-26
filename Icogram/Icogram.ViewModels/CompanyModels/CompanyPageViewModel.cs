using System.Collections.Generic;
using Icogram.Models.CompanyModels;
using Icogram.Models.UserModels;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.CompanyModels
{
    public class CompanyPageViewModel : LayoutViewModel
    {
        public List<Company> Companies { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}