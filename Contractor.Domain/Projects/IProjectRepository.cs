using Contractor.Tools;

namespace Contractor.Projects
{
    public interface IProjectRepository
    {
        Task<Project> CreateAsync(Project project);

        Task<Project> GetAsync(int id);

        Task<List<Project>> GetAllLiteAsync(string userId, string? projectNumber);

        Task<ListRepositoryModel<Project>> GetAllAsync(string userId, string? projectNumber, int? projectTypdId, string? ownerName, string? projectTitle, DateTime? startDate, DateTime? endDate, string? description, ProjectStatus? status, int pageNumber, int pageSize);
        
        public Task<bool> CheckProjectNumberExists(string consultantId, string projectNumber);

        Task<int> SaveChangesAsync();
    }
}
