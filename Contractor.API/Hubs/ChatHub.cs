using AutoMapper;
using Contractor.Chathub;
using Contractor.Files;
using Contractor.Identities;
using Contractor.Tools.Email;
using Contractor.ViewModels.Chathub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Contractor.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageAppService _chatMessageAppService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;

        public ChatHub(IChatMessageAppService chatMessageAppService, IUserRepository userRepository, IMapper mapper, IFileRepository fileRepository)
        {
            _chatMessageAppService = chatMessageAppService;
            _userRepository = userRepository;
            _mapper = mapper;
            _fileRepository = fileRepository;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatMessageConstants.GroupName);
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChatMessageConstants.GroupName);
            var user = _chatMessageAppService.GetUserByConnectionId(Context.ConnectionId);
            _chatMessageAppService.RemoveUserFromList(user);

            await DisplayOnlineUsers();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(ChatUserDto user)
        {
            _chatMessageAppService.AddUserConnectionId(user, Context.ConnectionId);
            await DisplayOnlineUsers();
        }

        public async Task ReceiveMessage(ChatMessageViewModel message)
        {
            await Clients.Group(ChatMessageConstants.GroupName).SendAsync("NewMessage", message);
        }

        public async Task CreatePrivateChat([FromForm]ChatMessageViewModel message)
        {
            string privateGroupName = GetPrivateGroupName(message.FromUserId, message.ToUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatMessageAppService.GetConnectionIdByUser(message.ToUserId);
            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

            var messgeDto = _mapper.Map<ChatMessageDto>(message);
            messgeDto.FromUserName = message.FromUserName;

            if(message.MediaFile != null)
            {
                await _fileRepository.SaveChangesAsync();
            }

            messgeDto = await _chatMessageAppService.CreateAsync(messgeDto);
            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", messgeDto);
        }

        public async Task ReceivePrivateMessage([FromForm] ChatMessageViewModel message)
        {
            string privateGroupName = GetPrivateGroupName(message.FromUserId, message.ToUserId);

            var messgeDto = _mapper.Map<ChatMessageDto>(message);
            messgeDto.FromUserName = message.FromUserName;
            messgeDto = await _chatMessageAppService.CreateAsync(messgeDto);
            messgeDto.FromUserName = (await _userRepository.GetByIdAsync(messgeDto.FromUserId)).DisplayName;
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", messgeDto);
        }

        public async Task RemovePrivateChat(string from, string to)
        {
            if(from != null && to != null)
            {
                string privateGroupName = GetPrivateGroupName(from, to);
                await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
                var toConnectionId = _chatMessageAppService.GetConnectionIdByUser(to);
                await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
            }
        }

        private async Task DisplayOnlineUsers()
        {
            var onlineUsers = _chatMessageAppService.GetOnlineUsers();
            await Clients.Group(ChatMessageConstants.GroupName).SendAsync("OnlineUsers", onlineUsers);
        }

        private string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
        }
    }
}
