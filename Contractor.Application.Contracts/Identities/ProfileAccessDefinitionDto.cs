using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class ProfileAccessDefinitionDto
    {
        public int Id { get; set; }

        public ProfileAccessDefinitionType Type { get; set; }

        public string UserId { get; set; } = null!;

        public int AccessDefinitionId { get; set; }
    }
}
