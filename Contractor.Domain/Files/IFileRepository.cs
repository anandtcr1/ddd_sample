using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public interface IFileRepository
    {
        Task<File> GetAsync(int id);

        Task<File> CreateAsync(File file);

        Task DeleteAsync(int id);

        Task DeleteAsync(List<int> ids);

        Task<int> SaveChangesAsync();
    }
}
