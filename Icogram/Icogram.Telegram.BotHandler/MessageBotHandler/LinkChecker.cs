using System;
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


        public async Task MessageCheck(Update update, Message message, Chat chat, ITelegramBotClient telegramBotClient)
        {
            await LinkCheck(chat, message, telegramBotClient, update.Message.Text.ToLower(), update.Message.Entities);
        }

        public async Task CaptionCheck(Update update, Chat chat, ITelegramBotClient telegramBotClient)
        {
            await LinkCheck(chat, update.Message, telegramBotClient, update.Message.Caption.ToLower(), update.Message.Entities);
        }

        private async Task LinkCheck(Chat chat, Message tmessage, ITelegramBotClient telegramBotClient, string message, List<MessageEntity> messageEntities)
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
                            var chatMember =
                                await telegramBotClient.GetChatMemberAsync(chat.TelegramChatId, tmessage.From.Id);
                            if (chatMember.Status != ChatMemberStatus.Administrator && chatMember.Status != ChatMemberStatus.Creator)
                            {
                                isNeedToDelete = true;
                            }
                        }
                    }

                    if (isNeedToDelete)
                    {
                        var users = await _suspiciousUserCrudService.GetAllAsync();
                        var user =
                            users.FirstOrDefault(u => u.TelegramUserId == tmessage.From.Id && u.ChatId == chat.Id);
                        if (user == null)
                        {
                            user = new SuspiciousUser
                            {
                                FirstName = tmessage.From.FirstName ?? "",
                                LastName = tmessage.From.LastName ?? "",
                                UserName = tmessage.From.Username ?? "",
                                ChatId = chat.Id,
                                NumberOfAttempts = 1,
                                TelegramUserId = tmessage.From.Id
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

                        await telegramBotClient.DeleteMessageAsync(tmessage.Chat.Id, tmessage.MessageId);
                        try
                        {
                            await _statisticHandler.AddDeletedMessageAsync(chat.Id);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e.StackTrace, $"{Errors.StatisticErorr}: {Errors.AddDeletedMessageError}");
                        }
                        
                        if (!string.IsNullOrEmpty(setting.WarningMessage))
                        {
                            var mess = new StringBuilder(setting.WarningMessage);
                            ParamsSetter.SetUserParams(ref mess, tmessage.From);
                            mess.Replace("[NumberOfAttempts]", user.NumberOfAttempts.ToString());
                            await
                                telegramBotClient.SendTextMessageAsync(tmessage.Chat.Id, mess.ToString());
                        }

                        if (setting.IsNeededToBanUser && setting.NumberOfAttempts <= user.NumberOfAttempts)
                        {
                            await BanUserAsync(user, chat, telegramBotClient);
                        }

                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.StackTrace, "Link check - " + Newtonsoft.Json.JsonConvert.SerializeObject(setting));
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
                var suspUser = await _suspiciousUserCrudService.GetByIdAsync(user.Id);
                suspUser.IsUserBanned = true;
                suspUser.BannedDate = DateTime.UtcNow;
                await _suspiciousUserCrudService.UpdateAsync(suspUser);
                try
                {
                    await _statisticHandler.AddBannedUserAsync(chat.Id);
                }
                catch (Exception e)
                {
                    _logger.Error(e.StackTrace, $"{Errors.StatisticErorr}: {Errors.AddBannedUserError}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.StackTrace, "Ban user");
            }
        }

    }
}