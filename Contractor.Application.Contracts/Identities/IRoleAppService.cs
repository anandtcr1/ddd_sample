using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IRoleAppService
    {
        Task<RoleDto> GetAsync(string id);
        
        Task<ListServiceModel<RoleDto>> GetListAsync(string? name,
            bool? isActive,
            int pageNumber, int pageSize);
        
        Task<RoleDto> CreateAsync(RoleDto roleDto);
        
        Task<RoleDto> UpdateAsync(RoleDto roleDto);
        
        Task<RoleDto> UpdateRolePagesAsync(string roleId, List<int> pageIds);
    }
}
