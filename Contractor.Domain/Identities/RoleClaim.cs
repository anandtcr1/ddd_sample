using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public RoleClaim() { }

        private RoleClaim(string? roleId, int? pageId,string claimType, string claimValue)
        {
            RoleId = roleId;
            PageId = pageId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public int? PageId { get; private set; }

        public virtual Page? Page { get; private set; }


        public static RoleClaim Create(string? roleId, int? pageId, string claimType, string claimValue)
        {
            return new RoleClaim(roleId, pageId, claimType, claimValue);
        }
    }
}
