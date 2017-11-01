using System.Collections.Generic;
using Icogram.Models.ResourcesModels;
using Icogram.ViewModels.User;

namespace Icogram.ViewModels.Layout
{
    public class LayoutViewModel
    {
        public UserProfileViewModel UserProfileViewModel { get;set; }

        public List<Resource> Resources { get; set; }
    }
}