using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Enums;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Models.ResourcesModels;
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
        private readonly ICrudService<Resource> _resourceCrudService;


        public CommandHandler(CrudService<Command> commandCrudService, IStatisticHandler statisticHandler, Logger logger, ICrudService<Resource> resourceCrudService)
        {
            _commandCrudService = commandCrudService;
            _statisticHandler = statisticHandler;
            _logger = logger;
            _resourceCrudService = resourceCrudService;
            _telegramBotClient = IcogramBot.GetClient();
        }


        public async Task ExecuteCommandAsync(Update update, Chat chat)
        {
            var command =
                chat.Commands.FirstOrDefault(
                    c =>
                        $"/{c.CommandName.Trim()}" == update.Message.Text.Trim() ||
                        $"/{c.CommandName.Trim()}@{IcogramBotSettings.Name}" == update.Message.Text);
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
                                _logger.Error(e.InnerException, $"{Errors.StatisticErorr}: {Errors.AddCommandError}");
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
                            _logger.Error(e.InnerException, $"{Errors.StatisticErorr}: {Errors.AddCommandError}");
                        }
                    }
                }
            }
        }

        public async Task ShowListCommandsAsync(Update update, Chat chat)
        {
            var check = Checker.AccessCheck(GlobalEnums.ModuleType.CommandModule, chat);
            if (update.Message.Text.Trim() == $"/commands@{IcogramBotSettings.Name}" || update.Message.Text.Trim() == "/commands")
            {
                if (check)
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("You can use these commands: \n");
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
                        _logger.Error(e.InnerException, $"{Errors.StatisticErorr}: {Errors.AddCommandError}");
                    }
                }
                else
                {
                    var resources = await _resourceCrudService.GetAllAsync();
                    var answer = resources.FirstOrDefault(r => r.Name == "NoCommands");
                    if (!string.IsNullOrEmpty(answer?.DefaultValue))
                    {
                        await _telegramBotClient.SendTextMessageAsync(chat.TelegramChatId, answer.DefaultValue);
                    }
                }

            }
        }
    }
}