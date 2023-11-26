using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class RoleManager : RoleManager<Role>, IRoleManager
    {
        private readonly IRoleRepository _roleRepository;
        public RoleManager(IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger,
            IRoleRepository roleRepository) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _roleRepository.GetRoleByNameAsync(name);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            IdentityResult result = await base.CreateAsync(role);
            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));

                throw new CustomValidationException(nameof(Role), errors);
            }

            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            await base.UpdateAsync(role);
            return role;
        }
    }
}
