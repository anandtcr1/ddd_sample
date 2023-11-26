using Contractor.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Chathub
{
    public class ChatUserDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public UserTypes UserType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ConnectionId { get; set; }

    }
}
