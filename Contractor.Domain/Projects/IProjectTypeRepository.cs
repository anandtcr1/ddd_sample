using Contractor.Tools;

namespace Contractor.Projects
{
    public interface IProjectTypeRepository
    {
        Task<ProjectType> CreateAsync(ProjectType projectType);
        Task<ProjectType> GetAsync(int id);
        Task<List<ProjectType>> GetAllAsync(string? search);
        Task<ListRepositoryModel<ProjectType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
