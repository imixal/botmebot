using System.Collections.Generic;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.Command
{
    public class CommandPageViewModel : LayoutViewModel
    {
        public List<Models.ModuleModels.CommandModule.Command> Commands { get; set; }
    }
}