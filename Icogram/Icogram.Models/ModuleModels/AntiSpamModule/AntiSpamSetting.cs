using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.AntiSpamModule
{
    public class AntiSpamSetting : Entity
    {
        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public bool IsModuleIncluded { get; set; }

        public bool IsInvertMode { get; set; }

        public bool IsNeededToBanUser { get; set; }

        public int NumberOfAttempts { get; set; }

        public string WarningMessage { get; set; }
    }
}