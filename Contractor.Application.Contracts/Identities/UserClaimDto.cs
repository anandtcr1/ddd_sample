using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class UserClaimDto
    {
        public int Id { get; set; }
        
        public int? PageId { get; set; }

        public string ClaimValue { get; set; }

        public string ClaimType { get; set; }
    }
}
