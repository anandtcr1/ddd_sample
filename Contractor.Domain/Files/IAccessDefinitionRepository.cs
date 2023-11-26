using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public interface IAccessDefinitionRepository
    {
        Task<AccessDefinition> GetAsync(int id);
        
        Task<List<AccessDefinition>> GetAsync(List<int> ids);

        Task<List<AccessDefinition>> GetRouteAsync(string userId);
        
        Task<List<AccessDefinition>> GetByPathAsync(string userId, string path);

        Task<AccessDefinition> GetByPathAsync(string path);

        Task<List<AccessDefinition>> GetByParentIdAsync(int parentId);

        Task<AccessDefinition> CreateAsync(AccessDefinition file);
        
        Task<List<AccessDefinition>> CreateAsync(List<AccessDefinition> accessDefinitionList);

        Task DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
