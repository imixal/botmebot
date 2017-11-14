using System;
using Icogram.Enums;
using Icogram.Models.ChatModels;

namespace Icogram.Telegram.BotHandler
{
    public static class Checker
    {
        internal static bool AccessCheck(GlobalEnums.ModuleType type, Chat chat)
        {
            const bool result = false;
            if (!chat.IsApproved) return result;
            if (!chat.Company.End.HasValue) return result;
            if (chat.Company.End.Value.Date < DateTime.UtcNow.Date) return result;
            switch (type)
            {
                case GlobalEnums.ModuleType.NotSet:
                    return true;
                case GlobalEnums.ModuleType.WelcomeMessageModule:
                    return chat.Company.IsWelcomeMessageModuleActivated;
                case GlobalEnums.ModuleType.CommandModule:
                    return chat.Company.IsCommandModuleActivated;
                case GlobalEnums.ModuleType.AntiSpamModule:
                    return chat.Company.IsAntiSpamModuleActivated;
                case GlobalEnums.ModuleType.CustomMessageModule:
                    return chat.Company.IsCustomMessageModuleActivated;
                default:
                    return result;
            }
        }
    }
}