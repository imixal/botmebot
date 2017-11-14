using System;
using Icogram.Enums;
using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;
using Icogram.Models.FileModel;

namespace Icogram.Models.ModuleModels.CommandModule
{
    public class Command : Entity
    {
        public string CommandName { get; set; }

        public string CommandDescription { get; set; }

        public string ActionMessage { get; set; }

        public string CommandParams { get; set; }

        public GlobalEnums.CommandType Type { get; set; }

        public int? TelegramFileId { get; set; }

        public File File { get; set; }

        public int? FileId { get; set; }

        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public bool IsCommandShowInList { get; set; }

        public DateTime? LastUsage { get; set; }
    }
}