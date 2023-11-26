
using Contractor.Tools;

namespace Contractor.Correspondences
{
    public interface ICorrespondenceRepository
    {
        Task<CorrespondenceThread> CreateThreadAsync(CorrespondenceThread correspondenceThread);
        
        Task<Correspondence> CreateAsync(Correspondence correspondence);
        
        Task<Correspondence> GetAsync(int id);
        
        Task<List<Correspondence>> GetByThreadIdAsync(int threadId);
        
        Task<ListRepositoryModel<Correspondence>> GetIncomingAsync(string userId, string? search, int pageNumber, int pageSize);
        
        Task<ListRepositoryModel<Correspondence>> GetOutgoingAsync(string userId, string? search, int pageNumber, int pageSize);

        Task<int> GetNextIdAsync();

        Task<int> SaveChangesAsync();

    }
}
