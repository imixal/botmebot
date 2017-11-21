using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Async;
using Icogram.DataAccessLayer.Interfaces;
using Icogram.DbContext;
using Icogram.Models.ModuleModels.StatisticsModule;

namespace Icogram.DataAccessLayer.Repository
{
    public class ChatStatisticRepository : Repository<ChatStatistic>, IChatStatisticRepository
    {
        private static ChatStatistic _chatStatisticRow;


        public ChatStatisticRepository(IcogramDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddNewMessage(int chatId)
        {
            var row = await GetStatRow(chatId);
            row.NumberOfMessages++;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddDeletedMessage(int chatId)
        {
            var row = await GetStatRow(chatId);
            row.NumberOfDeletedMessages++;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddNewUsers(int chatId, int count)
        {
            var row = await GetStatRow(chatId);
            row.NumberOfNewUsers+=count;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddLeavedUser(int chatId)
        {
            var row = await GetStatRow(chatId);
            row.NumberOfLeavedUsers++;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddBannedUser(int chatId)
        {
            var row = await GetStatRow(chatId);
            row.NumberOfBannedUsers++;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddCommand(int chatId)
        {
            var row = await GetStatRow(chatId);
            row.NumberOfCommands++;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddSymbolsInMessage(int chatId, int value)
        {

            var row = await GetStatRow(chatId);
            row.NumberOfSymbolsInMessage += value;
            Update(row);
            await DbContext.SaveChangesAsync();
        }

        public async Task<List<ChatStatistic>> GetChatStatistic(int chatId)
        {
            var stat = GetAllQuery().Where(cs => cs.ChatId == chatId);

            return await stat.ToListAsync();
        }


        private async Task<ChatStatistic> GetStatRow(int chatId)
        {
            var now = DateTime.UtcNow;
            var chatStatisticRow =
                GetAllQuery().Where(cs => DbFunctions.TruncateTime(cs.Date) == DbFunctions.TruncateTime(now)).FirstOrDefault(cs => cs.ChatId == chatId);
            if (chatStatisticRow != null)
            {
                _chatStatisticRow = chatStatisticRow;
            }
            else
            {
                var newChatStatisticRow = new ChatStatistic
                {
                    Date = now.Date,
                    ChatId = chatId,
                    NumberOfBannedUsers = 0,
                    NumberOfCommands = 0,
                    NumberOfDeletedMessages = 0,
                    NumberOfLeavedUsers = 0,
                    NumberOfMessages = 0,
                    NumberOfNewUsers = 0,
                    NumberOfSymbolsInMessage = 0
                };
                Create(newChatStatisticRow);
                await DbContext.SaveChangesAsync();
                _chatStatisticRow = newChatStatisticRow;
            }

            return _chatStatisticRow;
        }
    }
}
