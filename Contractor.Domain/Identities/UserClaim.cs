using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class UserClaim: IdentityUserClaim<string>
    {
        public UserClaim()
        {
            
        }

        private UserClaim(string userId, int? pageId, string claimType, string claimValue)
        {
            PageId = pageId;
            ClaimType = claimType;
            ClaimValue = claimValue;
            UserId = userId;
        }

        public int? PageId { get; private set; }

        public virtual Page? Page { get; private set; }

        
        
        public static UserClaim Create(string userId, int? pageId, string claimType, string claimValue)
        {
            return new UserClaim(userId, pageId, claimType, claimValue);
        }
    }
}
