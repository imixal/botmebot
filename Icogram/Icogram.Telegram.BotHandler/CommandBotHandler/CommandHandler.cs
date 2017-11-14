using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Enums;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Telegram.Bot.Bot;
using Icogram.Telegram.BotHandler.StatisticBotHandler;
using NLog;
using Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler.CommandBotHandler
{
    public class CommandHandler : ICommandHandler
    {
        private readonly CrudService<Command> _commandCrudService;
        private readonly TelegramBotClient _telegramBotClient;
        private readonly IStatisticHandler _statisticHandler;
        private readonly Logger _logger;


        public CommandHandler(CrudService<Command> commandCrudService, IStatisticHandler statisticHandler, Logger logger)
        {
            _commandCrudService = commandCrudService;
            _statisticHandler = statisticHandler;
            _logger = logger;
            _telegramBotClient = IcogramBot.GetClient();
        }


        public async Task ExecuteCommandAsync(Update update, Chat chat)
        {
            var command =
                chat.Commands.FirstOrDefault(
                    c =>
                        $"/{c.CommandName}" == update.Message.Text ||
                        $"/{c.CommandName}@{IcogramBotSettings.Name}" == update.Message.Text);
            var check = Checker.AccessCheck(GlobalEnums.ModuleType.CommandModule, chat);
            if (check)
            {
                if (!string.IsNullOrEmpty(command?.ActionMessage))
                {
                    var actionMessageStrBuilder = new StringBuilder(command.ActionMessage);
                    ParamsSetter.SetUserParams(ref actionMessageStrBuilder, update.Message.From);
                    if (command.LastUsage.HasValue)
                    {
                        if (command.LastUsage.Value.AddSeconds(command.Chat.CommandTimeOut) <= DateTime.UtcNow)
                        {
                            await
                                _telegramBotClient.SendTextMessageAsync(chat.TelegramChatId,
                                    actionMessageStrBuilder.ToString(),
                                    replyToMessageId: update.Message.MessageId);
                            command = await _commandCrudService.GetByIdAsync(command.Id);
                            command.LastUsage = DateTime.UtcNow;
                            await _commandCrudService.UpdateAsync(command);
                            try
                            {
                                await _statisticHandler.AddCommandAsync(chat.Id);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddCommandError}");
                            }
                        }

                    }
                    else
                    {
                        await
                            _telegramBotClient.SendTextMessageAsync(chat.TelegramChatId, actionMessageStrBuilder.ToString(),
                                replyToMessageId: update.Message.MessageId);
                        command = await _commandCrudService.GetByIdAsync(command.Id);
                        command.LastUsage = DateTime.UtcNow;
                        await _commandCrudService.UpdateAsync(command);
                        try
                        {
                            await _statisticHandler.AddCommandAsync(chat.Id);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddCommandError}");
                        }
                    }
                }
            }
        }

        public async Task ShowListCommandsAsync(Update update, Chat chat)
        {
            var check = Checker.AccessCheck(GlobalEnums.ModuleType.CommandModule, chat);
            if (update.Message.Text == $"/commands@{IcogramBotSettings.Name}" || update.Message.Text == "/commands")
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("You can use these commands: \n");
                if (check)
                {
                    foreach (var command in chat.Commands)
                    {
                        if (command.IsCommandShowInList)
                        {
                            stringBuilder.Append($"/{command.CommandName} - {command.CommandDescription} \n");
                        }
                    }
                    await _telegramBotClient.SendTextMessageAsync(chat.TelegramChatId, stringBuilder.ToString());
                    try
                    {
                        await _statisticHandler.AddCommandAsync(chat.Id);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"{Errors.StatisticErorr}: {Errors.AddCommandError}");
                    }
                }
            }
        }
    }
}