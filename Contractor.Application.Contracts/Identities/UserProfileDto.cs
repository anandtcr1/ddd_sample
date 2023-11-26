using Contractor.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class UserProfileDto
    {
        public string UserId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string? Address { get; set; }

        public Gender Gender { get; set; }

        public string? AlternativeEmail { get; set; }


        public int AreaId { get; set; }

        public int NationalityId { get; set; }

        public List<ProfileAccessDefinitionDto> ProfileAccessDefinitions { get; set; }

        public List<int> Attachments { get; set; }

        public AreaDto? Area { get; set; }
    }
}
