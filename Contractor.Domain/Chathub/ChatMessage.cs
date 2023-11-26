using Contractor.Identities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{
    public class ChatMessage
    {
        private ChatMessage(string fromUserId, string toUserId, string content, MessageTypeEnum messageType)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Content = content;
            MessageType = messageType;
        }

        [Key]
        public int Id { get; private set; }

        [ForeignKey("User")]
        public string FromUserId { get; private set; }
        
        [ForeignKey("User")]
        public string ToUserId { get; private set; }
        
        public string Content { get; private set; }
        
        public DateTime MessageDate { get; private set; } = DateTime.Now;

        public MessageTypeEnum MessageType { get; set; }

        public virtual User FromUser { get; set; }

        public virtual User ToUser { get; set; }

        public static ChatMessage Create(string fromUserId, string toUserId, string content, MessageTypeEnum messageType)
        {
            return new ChatMessage(fromUserId, toUserId, content, messageType);
        }
    }
}
