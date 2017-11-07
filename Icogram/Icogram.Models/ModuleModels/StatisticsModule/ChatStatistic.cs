using System;
using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.ModuleModels.StatisticsModule
{
    public class ChatStatistic : Entity
    {
        public Chat Chat { get; set; }

        public int ChatId { get; set; }

        public DateTime Date { get; set; }

        public int NumberOfMessages { get; set; }

        public int NumberOfDeletedMessages { get; set; }

        public int NumberOfNewUsers { get; set; }

        public int NumberOfLeavedUsers { get; set; }

        public int NumberOfBannedUsers { get; set; }

        public int NumberOfCommands { get; set; }

        public long NumberOfSymbolsInMessage { get; set; }
    }
}