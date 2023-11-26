using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public interface IProjectAppService
    {
        Task<ProjectDto> CreateAsync(ProjectDto projectDto, string consultantId, string consultantName, string? ownerId);

        Task<ProjectDto> UpdateAsync(ProjectDto projectDto);
        
        Task AddProjectSubConsultant(int projectId, string userId, List<string> subConsultantIdList);
        
        Task AddProjectContractor(int projectId, string userId, List<string> contractorIdList);
        
        Task AddProjectSubContractor(int projectId, string userId, List<string> subContractorIdList);
        
        Task AddSubUser(int projectId, string userId, List<string> subUserIdList);

        Task<ProjectInvitationResponse> InviteProjectOwner(int projectId, string userId, string email, string redirectUrl);

        Task<ProjectDto> GetAsync(int id, string userId);
        
        Task<List<ProjectUserListDto>> GetProjectUsersAsync(int id, string userId);
        
        Task<ProjectDto> CheckProjectInvitationAsync(int projectId, string email, string userId);
        
        Task AcceptProjectInvitationAsync(int projectId, string userId);
        
        Task DeclineProjectInvitationAsync(int projectId, string userId);

        Task<ListServiceModel<ProjectsListDto>> GetAllAsync(string userId, string? projectNumber, int? projectTypdId, string? ownerName, string? projectTitle, DateTime? startDate, DateTime? endDate, string? description, ProjectStatus? status, int pageNumber, int pageSize);

    }
}
