using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public interface IProjectTypeAppService
    {
        Task<ProjectTypeDto> CreateAsync(ProjectTypeDto projectTypeDto);

        Task<ProjectTypeDto> UpdateAsync(ProjectTypeDto projectTypeDto);

        Task<ProjectTypeDto> GetAsync(int id);

        Task<ListServiceModel<ProjectTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize);
    }
}
