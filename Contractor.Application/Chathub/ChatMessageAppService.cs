using AutoMapper;
using Contractor.Files;
using Contractor.Identities;
using Contractor.Projects;
using Contractor.Tenders;
using Contractor.Tools;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{

    public class ChatMessageAppService : IChatMessageAppService
    {
        private static readonly List<ChatUserDto> Users = new List<ChatUserDto>();

        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IBlobManager _blobManager;
        private readonly IFileRepository _fileRepository;

        public ChatMessageAppService(IChatMessageRepository chatMessageRepository, IMapper mapper, IUserRepository userRepository, IBlobManager blobManager, IFileRepository fileRepository)
        {
            _chatMessageRepository = chatMessageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _blobManager = blobManager;
            _fileRepository = fileRepository;
        }

        public bool AddUserToList(ChatUserDto userToAdd)
        {
            lock (Users)
            {
                var isUserExists = false;
                foreach (var user in Users)
                {
                    if (user.Id == userToAdd.Id)
                    {
                        isUserExists = true;
                    }
                }

                if (!isUserExists)
                {
                    Users.Add(userToAdd);
                }
                return true;
            }
        }

        public void AddUserConnectionId(ChatUserDto user, string connectionId)
        {
            lock (Users)
            {
                if(user!= null)
                {
                    var userIndex = Users.FindIndex(x => x.Id == user.Id);
                    if (userIndex > -1)
                    {
                        Users[userIndex].ConnectionId = connectionId;
                    }
                }
            }
        }

        public ChatUserDto GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                return Users.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(string userId)
        {
            lock (Users)
            {
                return Users.Where(x => x.Id == userId).Select(x => x.ConnectionId).FirstOrDefault();
            }
        }

        public void RemoveUserFromList(ChatUserDto user)
        {
            lock (Users)
            {
                if(user != null)
                {
                    var userIndex = Users.FindIndex(x => x.Id == user.Id);
                    if (userIndex > -1)
                    {
                        Users.Remove(user);
                    }
                }
            }
        }

        public ChatUserDto[] GetOnlineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(x => x.DisplayName).ToArray();
            }
        }

        public async Task<ChatMessageDto> CreateAsync(ChatMessageDto chatMessageDto)
        {
            var chat = ChatMessage.Create(chatMessageDto.FromUserId, chatMessageDto.ToUserId, chatMessageDto.Content, chatMessageDto.MessageType);
            await _chatMessageRepository.CreateAsync(chat);
            await _chatMessageRepository.SaveChangesAsync();
            return _mapper.Map<ChatMessageDto>(chat);
        }

        public async Task<ListServiceModel<ChatMessageDto>> GetAllChatsAsync(string fromUserId, string toUserId, int pageNumber, int pageSize)
        {
            var chatList = await _chatMessageRepository.GetAllChatsAsync(fromUserId, toUserId, pageNumber, pageSize);

            var chatListDto = _mapper.Map<List<ChatMessageDto>>(chatList.List);
            foreach(var chat in chatListDto)
            {
                chat.FromUserName = (await _userRepository.GetByIdAsync(chat.FromUserId)).DisplayName;
            }

            return new ListServiceModel<ChatMessageDto>(chatList.TotalCount, chatListDto);
        }

        public async Task<AccessDefinitionDto> UploadChatFile(string fromUserId , string email ,IFormFile mediaFile)
        {
            AccessDefinition accessDefinition = await _blobManager.UploadProfileChatFiles(fromUserId, email, mediaFile);

            await _fileRepository.SaveChangesAsync();

            return _mapper.Map<AccessDefinitionDto>(accessDefinition);
        }
    }
}
