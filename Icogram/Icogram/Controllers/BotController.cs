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
            if (!await _botHandler.IsChatApprovedAsync(update.Message.Chat.Id)) return Ok();
            await _botHandler.MessageHandler(update);

            return Ok();
        }
    }
}
