using Contractor.Files;
using Contractor.Tools;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{
    public interface IChatMessageAppService
    {
        bool AddUserToList(ChatUserDto userToAdd);
        void AddUserConnectionId(ChatUserDto user, string connectionId);
        ChatUserDto GetUserByConnectionId(string connectionId);
        string GetConnectionIdByUser(string userId);
        void RemoveUserFromList(ChatUserDto user);
        ChatUserDto[] GetOnlineUsers();

        Task<ChatMessageDto> CreateAsync(ChatMessageDto chat);
        Task<ListServiceModel<ChatMessageDto>> GetAllChatsAsync(string fromUserId, string toUserId, int pageNumber, int pageSize);
        Task<AccessDefinitionDto> UploadChatFile(string fromUserId, string email, IFormFile mediaFile);
    }
}
