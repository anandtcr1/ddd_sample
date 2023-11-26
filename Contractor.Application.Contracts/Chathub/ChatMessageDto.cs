using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{
    public class ChatMessageDto
    {
        public string FromUserId { get; set; }
        public string? ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime MessageDate { get; set; }
        public string FromUserName { get; set; }
        public int Id { get; set; }
        public MessageTypeEnum MessageType { get; set; }
    }
}
