using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class RoleDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public bool IsActive { get; set; }
        
        public List<RoleClaimDto>? Claims { get; set; }
    }
}
