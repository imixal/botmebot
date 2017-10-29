using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icogram.ViewModels.Command
{
    public class CreateMyCommandViewModel
    {
        public string CommandName { get; set; }

        public string ActionMessage { get; set; }

        public int ChatId { get; set; }

        public bool IsCommandShowInList { get; set; }
    }
}