using System.Threading.Tasks;
using Telegram.Bot;

namespace Icogram.Telegram.Bot.Bot
{
    public class IcogramBot
    {
        private static TelegramBotClient _client;
        private const string Url = "api/botIcogramV1/update";

        public static async Task SetWebhookAsync()
        {
            var hook = string.Format(IcogramBotSettings.Url, Url);
            GetClient();
            await _client.SetWebhookAsync(hook);
        }

        public static TelegramBotClient GetClient()
        {
            if (_client != null)
            {
                return _client;
            }

            _client = new TelegramBotClient(IcogramBotSettings.Key);

            return _client;
        }
    }
}
