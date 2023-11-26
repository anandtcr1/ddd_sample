using Contractor.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IRoleManager
    {
        Task<Role> CreateRoleAsync(Role role);
        
        Task<Role> UpdateRoleAsync(Role role);       

        Task<Role?> GetRoleByNameAsync(string name);
    }
}
