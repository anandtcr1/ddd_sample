using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public interface IDraftProjectAppService
    {
        Task<DraftProjectDto> CreateAsync(DraftProjectDto draftProjectDto);

        Task<DraftProjectDto> GetAsync(int id);

        Task<ListServiceModel<DraftProjectListDto>> GetAllAsync(string consultantId, string? ownerName, DraftProjectStatus? statusId, DateTime? createdDate, int pageNumber, int pageSize);

        Task<DraftProjectDto> ChangeProjectStatusAsync(int id, DraftProjectStatus statusId);

    }
}
