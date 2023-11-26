using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Contractor.Tools.Email
{
    public class Message
    {
        public List<MailboxAddress> To { get; private set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }
        private Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }

        public static Message Create(IEnumerable<string> to, string subject, string content)
        {
            return new Message(to, subject, content);
        }
    }
}
