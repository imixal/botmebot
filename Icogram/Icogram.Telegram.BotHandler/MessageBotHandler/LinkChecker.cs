﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.Telegram.BotHandler.StatisticBotHandler;
using NLog;
using Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Chat = Icogram.Models.ChatModels.Chat;

namespace Icogram.Telegram.BotHandler.MessageBotHandler
{
    public class LinkChecker : ILinkChecker
    {
        private readonly ICrudService<WhiteLink> _whiteLinkCrudService;
        private readonly ICrudService<AntiSpamSetting> _antiSpamSettingsCrudService;
        private readonly ICrudService<SuspiciousUser> _suspiciousUserCrudService;
        private readonly IStatisticHandler _statisticHandler;
        private readonly Logger _logger;


        public LinkChecker(ICrudService<WhiteLink> whiteLinkCrudService,
            ICrudService<AntiSpamSetting> antiSpamSettingsCrudService,
            ICrudService<SuspiciousUser> suspiciousUserCrudService, IStatisticHandler statisticHandler, Logger logger)
        {
            _whiteLinkCrudService = whiteLinkCrudService;
            _antiSpamSettingsCrudService = antiSpamSettingsCrudService;
            _suspiciousUserCrudService = suspiciousUserCrudService;
            _statisticHandler = statisticHandler;
            _logger = logger;
        }


        public async Task MessageCheck(Update update, Chat chat, ITelegramBotClient telegramBotClient)
        {
            await LinkCheck(chat, update, telegramBotClient, update.Message.Text.ToLower(), update.Message.Entities);
        }

        public async Task CaptionCheck(Update update, Chat chat, ITelegramBotClient telegramBotClient)
        {
            await LinkCheck(chat, update, telegramBotClient, update.Message.Caption.ToLower(), update.Message.Entities);
        }

        private async Task LinkCheck(Chat chat, Update update, ITelegramBotClient telegramBotClient, string message, List<MessageEntity> messageEntities)
        {
            var whiteLinks = await _whiteLinkCrudService.GetAllAsync();
            var settings = await _antiSpamSettingsCrudService.GetAllAsync();
            var setting = settings.FirstOrDefault(s => s.ChatId == chat.Id);
            if (setting != null && setting.IsModuleIncluded)
            {

                whiteLinks = whiteLinks.Where(wl => wl.ChatId == chat.Id).ToList();
                try
                {
                    var isNeedToDelete = false;

                    foreach (var entity in messageEntities.Where(e => e.Type == MessageEntityType.Url))
                    {
                        var item = message.Substring(entity.Offset, entity.Length);
                        var link = "";
                        if (!item.Contains("http") && !item.Contains("https") && !item.Contains("www"))
                        {
                            link = "www." + item;
                        }
                        else
                        {
                            var url = item.StartsWith("www.") ? new Uri("http://" + item) : new Uri(item);
                            link = url.Host;
                        }
                        var isWhiteLink = false;
                        if (!string.IsNullOrEmpty(link) && link.Contains("."))
                        {
                            var words = link.Split('.');
                            if (words.Length > 1)
                            {
                                var word = words[words.Length - 2].ToLower();
                                var isAny = whiteLinks.Any(wl => wl.Link.ToLower() == word);
                                if (isAny && whiteLinks.Count > 0)
                                {
                                    isWhiteLink = true;
                                }
                            }
                        }
                        if (!isWhiteLink)
                        {
                            isNeedToDelete = true;
                        }
                    }

                    if (isNeedToDelete)
                    {
                        var users = await _suspiciousUserCrudService.GetAllAsync();
                        var user =
                            users.FirstOrDefault(u => u.TelegramUserId == update.Message.From.Id && u.ChatId == chat.Id);
                        if (user == null)
                        {
                            user = new SuspiciousUser
                            {
                                FirstName = update.Message.From.FirstName ?? "",
                                LastName = update.Message.From.LastName ?? "",
                                UserName = update.Message.From.Username ?? "",
                                ChatId = chat.Id,
                                NumberOfAttempts = 1,
                                TelegramUserId = update.Message.From.Id
                            };

                            await _suspiciousUserCrudService.CreateAsync(user);
                        }
                        else
                        {
                            user.NumberOfAttempts++;
                            var oldUser = await _suspiciousUserCrudService.GetByIdAsync(user.Id);
                            oldUser.NumberOfAttempts = user.NumberOfAttempts;
                            await _suspiciousUserCrudService.UpdateAsync(oldUser);
                        }

                        await telegramBotClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                        try
                        {
                            await _statisticHandler.AddDeletedMessageAsync(chat.Id);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e.InnerException, $"{Errors.StatisticErorr}: {Errors.AddDeletedMessageError}");
                        }
                        var mess = new StringBuilder(setting.WarningMessage);
                        ParamsSetter.SetUserParams(ref mess, update.Message.From);
                        mess.Replace("[NumberOfAttempts]", user.NumberOfAttempts.ToString());
                        await
                            telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, mess.ToString());

                        if (setting.IsNeededToBanUser && setting.NumberOfAttempts <= user.NumberOfAttempts)
                        {
                            await BanUserAsync(user, chat, telegramBotClient);
                        }

                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.InnerException, "Link check");
                }
            }
        }

        private async Task BanUserAsync(SuspiciousUser user, Chat chat, ITelegramBotClient telegramBotClient)
        {
            try
            {
                await
                    telegramBotClient.KickChatMemberAsync(user.Chat.TelegramChatId, user.TelegramUserId,
                        DateTime.UtcNow.AddDays(30));
                user.IsUserBanned = true;
                user.BannedDate = DateTime.UtcNow;
                await _suspiciousUserCrudService.UpdateAsync(user);
                try
                {
                    await _statisticHandler.AddBannedUserAsync(chat.Id);
                }
                catch (Exception e)
                {
                    _logger.Error(e.InnerException, $"{Errors.StatisticErorr}: {Errors.AddBannedUserError}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.InnerException, "Ban user");
            }
        }

    }
}