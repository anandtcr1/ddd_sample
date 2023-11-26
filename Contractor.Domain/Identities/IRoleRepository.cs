using Contractor.Tools;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Contractor.Identities
{
    public interface IRoleRepository
    {
        Task<ListRepositoryModel<Role>> GetListAsync(string? name,
            bool? isActive,
            int pageNumber, int pageSize);

        Task<List<Role>> GetAsync(List<string> names);
        
        Task<Role?> GetRoleByNameAsync(string name);

        Task<Role?> GetRoleByIdAsync(string id);

        Task<List<Role>> GetRoleByIdAsync(List<string> ids);

        Task<List<Role>> GetAll(string search);

        Task<int> SaveChangesAsync();
    }
}
