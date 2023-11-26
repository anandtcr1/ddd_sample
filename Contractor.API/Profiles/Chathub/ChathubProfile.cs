using AutoMapper;
using Contractor.Chathub;
using Contractor.ViewModels.Chathub;

namespace Contractor.Profiles.Chathub
{
    public class ChathubProfile: Profile
    {
        public ChathubProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>();
            CreateMap<ChatMessageViewModel, ChatMessageDto>();
        }
    }
}
