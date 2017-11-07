using System.Configuration;

namespace Icogram.Telegram.Bot.Bot
{
    public class IcogramBotSettings
    {
        public static string Url { get; set; } = ConfigurationManager.AppSettings["BotWebHook"];

        public static string Name { get; set; } = ConfigurationManager.AppSettings["BotName"];

        public static string Key { get; set; } = ConfigurationManager.AppSettings["BotKey"];
    }
}