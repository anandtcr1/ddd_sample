using Contractor.EntityFrameworkCore;
using Contractor.Lookups;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly DatabaseContext _databaseContext;
        public ChatMessageRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ChatMessage> CreateAsync(ChatMessage chat)
        {
            await _databaseContext.ChatMessages.AddAsync(chat);
            return chat;
        }

        public async Task<ListRepositoryModel<ChatMessage>> GetAllChatsAsync(string fromUserId, string toUserId, int pageNumber, int pageSize)
        {
            var query = _databaseContext.ChatMessages
                            .Where(x => (x.FromUserId == fromUserId && x.ToUserId == toUserId) ||
                                   (x.FromUserId == toUserId && x.ToUserId == fromUserId))
                            .OrderByDescending(x => x.MessageDate)
                            .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .OrderBy(x => x.MessageDate)
                        .ToListAsync();
            return new ListRepositoryModel<ChatMessage>(totalRecords, list);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
