using System;
using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.CommandModule
{
    public class Command : Entity
    {
        public string CommandName { get; set; }

        public string ActionMessage { get; set; }

        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public bool IsCommandShowInList { get; set; }

        public DateTime? LastUsage { get; set; }
    }
}