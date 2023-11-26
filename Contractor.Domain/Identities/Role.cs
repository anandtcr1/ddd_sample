using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Contractor.Identities
{
    public class Role : IdentityRole<string>
    {
        public Role()
        {
            Claims = new HashSet<RoleClaim>();
            IsActive = true;
        }

        private Role(string name, bool isActive)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            IsActive = isActive;
            Claims = new HashSet<RoleClaim>();
        }

        public bool IsActive { get; private set; }

        public virtual ICollection<RoleClaim> Claims { get; private set; }
        
        public virtual ICollection<UserRole> Users { get; private set; }


        public static Role Create(string name, bool isActive)
        {
            return new Role(name, isActive);
        }

        public void Update(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }

        public void AddPageClaims(List<Tuple<int,string>> pages)
        {
            var pagesClaims = pages.Select(x => RoleClaim.Create(Id, x.Item1, CustomClaimTypes.Page, x.Item2)).ToList();

            foreach (var page in pagesClaims)
            {
                Claims.Add(page);
            }
        }

        public void UpdatePageClaims(List<Tuple<int, string>> pages)
        {
            Claims = Claims.Where(x=>x.ClaimType != CustomClaimTypes.Page).ToList();

            AddPageClaims(pages);
        }

        public void AddFunctionalityClaims(List<string> Functionalities)
        {
            var claims = Functionalities.Select(x => RoleClaim.Create(Id, null, CustomClaimTypes.Functionality, x)).ToList();

            foreach (var item in claims)
            {
                Claims.Add(item);
            }
        }

        public void UpdateFunctionalityClaims(List<string> Functionalities)
        {
            Claims = Claims.Where(x => x.ClaimType != CustomClaimTypes.Functionality).ToList();

            AddFunctionalityClaims(Functionalities);
        }
    }
}
