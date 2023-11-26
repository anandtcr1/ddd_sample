using Contractor.Chathub;
using Contractor.GeneralViewModels;
using Contractor.Identities;

namespace Contractor.ViewModels.Chathub
{
    public class GetChatMessageViewModel : GetListViewModel
    {
        public string ToUserId { get; set; }
    }

    public class ChatMessageViewModel
    {
        public string FromUserId { get; set; }
        public string? ToUserId { get; set; }
        public string Content { get; set; }
        public string FromUserName { get; set; }
        public IFormFile MediaFile { get; set; }
        public MessageTypeEnum MessageType { get; set; }
    }

    public class ChatMediaViewModel
    {
        public string FromUserId { get; set; }
        public string Email { get; set; }
        public IFormFile MediaFile { get; set; }
    }
}
