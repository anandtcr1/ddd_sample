using Contractor.Tools;

namespace Contractor.Correspondences
{
    public interface IOutGoingTypeRepository
    {
        Task<OutGoingType> CreateAsync(OutGoingType outGoingType);

        Task<OutGoingType> GetAsync(int id);

        Task<List<OutGoingType>> GetAllAsync(string? search);

        Task<ListRepositoryModel<OutGoingType>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
