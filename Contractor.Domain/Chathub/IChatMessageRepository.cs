using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{
    public interface IChatMessageRepository
    {
        Task<ChatMessage> CreateAsync(ChatMessage chat);
        Task<ListRepositoryModel<ChatMessage>> GetAllChatsAsync(string fromUserId, string toUserId, int pageNumber, int pageSize);
        Task<int> SaveChangesAsync();
    }
}
