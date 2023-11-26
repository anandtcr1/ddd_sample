using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public interface IDraftProjectRepository
    {
        Task<DraftProject> CreateAsync(DraftProject draftProject);
        Task<DraftProject> GetAsync(int id);
        Task<ListRepositoryModel<DraftProject>> GetAllAsync(string consultantId, string? ownerName, DraftProjectStatus? statusId, DateTime? createdDate, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
