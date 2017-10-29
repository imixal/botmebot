using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Icogram.Telegram.Bot.Bot;
using Icogram.Telegram.BotHandler;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Icogram.Controllers
{
    [Route(@"api/botIcogramV1/update")] //webhook uri part
    public class BotController : ApiController
    {
        private readonly IBotHandler _botHandler;


        public BotController(IBotHandler botHandler)
        {
            _botHandler = botHandler;
        }


        public async Task<OkResult> Update([FromBody]Update update)
        {
            var chat = await _botHandler.GetApprovedChatAsync(update.Message.Chat.Id);
            if (chat == null) return Ok();
            await _botHandler.MessageHandler(update, chat);

            return Ok();
        }
    }
}
