using System.Text;
using Telegram.Bot.Types;

namespace Icogram.Telegram.BotHandler
{
    public static class ParamsSetter
    {
        internal static void SetChatParams(ref StringBuilder message, Update update)
        {
            if (update.Message.Chat == null) return;
            message.Replace("[ChatUserName]",
                string.IsNullOrEmpty(update.Message.Chat.Username) ? "" : update.Message.Chat.Username);
            message.Replace("[ChatFirstName]",
                string.IsNullOrEmpty(update.Message.Chat.FirstName) ? "" : update.Message.Chat.FirstName);
            message.Replace("[ChatLastName]",
                string.IsNullOrEmpty(update.Message.Chat.LastName) ? "" : update.Message.Chat.LastName);
            message.Replace("[ChatTitle]",
                string.IsNullOrEmpty(update.Message.Chat.Title) ? "" : update.Message.Chat.Title);
            message.Replace("[ChatInviteLink]",
                string.IsNullOrEmpty(update.Message.Chat.InviteLink) ? "" : update.Message.Chat.InviteLink);
            message.Replace("[ChatDescription]",
                string.IsNullOrEmpty(update.Message.Chat.Description) ? "" : update.Message.Chat.Description);
        }

        internal static void SetUserParams(ref StringBuilder message, User user)
        {
            message.Replace("[FirstName]",
                    string.IsNullOrEmpty(user.FirstName)
                        ? ""
                        : user.FirstName);
            message.Replace("[LastName]",
                string.IsNullOrEmpty(user.LastName)
                    ? ""
                    : user.LastName);
            message.Replace("[UserName]",
                string.IsNullOrEmpty(user.Username)
                    ? ""
                    : user.Username);
        }
    }
}