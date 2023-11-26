using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class SubUserDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public UserTypes UserType { get; set; }

        public int CompanyId { get; set; }

        public UserProfileDto? Profile { get; set; }

        public List<UserClaimDto> Claims { get; set; }

        public DateTime InsertDate { get; set; }

        public string InsertUserId { get; set; }

        public string InsertUserName { get; set; }

        public UserStatus Status { get; set; }

    }
}
