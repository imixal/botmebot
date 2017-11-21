using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Icogram.Telegram.Bot.Bot;
using Icogram.Telegram.BotHandler;
using NLog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Icogram.Controllers
{
    [Route(@"api/botIcogramV3/update")] //webhook uri part
    public class BotController : ApiController
    {
        private readonly IBotHandler _botHandler;



        public BotController(IBotHandler botHandler)
        {
            _botHandler = botHandler;
        }

        protected override ExceptionResult InternalServerError(Exception exception)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error(exception);
            return base.InternalServerError(exception);
        }

        public async Task<OkResult> Update([FromBody]Update update)
        {
            if (update.Message == null) return Ok();
            var chat = await _botHandler.GetApprovedChatAsync(update.Message.Chat.Id);
            if (chat == null)
            {
                chat = await _botHandler.GetUnApprovedChatAsync(update.Message.Chat.Id);
                await _botHandler.UnApprovedChatHandler(update, chat);
                return Ok();
            }
            await _botHandler.MessageHandler(update, chat);

            return Ok();
        }
    }
}
