using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IPageRepository
    {
        Task<List<Page>> GetByName(List<string> names);
        
        Task<Page> GetAsync(int id);
        
        Task<List<Page>> GetListAsync(List<int> ids);
        
        Task<ListRepositoryModel<Page>> GetListAsync(string name, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
