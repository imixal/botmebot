using System.Collections.Generic;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.AntiSpam
{
    public class SuspiciousUsersPageViewModel : LayoutViewModel
    {
        public List<SuspiciousUser> SuspiciousUsers { get; set; }
    }
}