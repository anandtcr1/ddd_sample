using Contractor.Tools;

namespace Contractor.Lookups
{
    public interface INationalityRepository
    {
        Task<Nationality> GetAsync(int id);

        Task<Nationality> CreateAsync(Nationality nationality);

        Task<List<Nationality>> GetAllAsync(string? search);

        Task<ListRepositoryModel<Nationality>> GetAllAsync(string? arabicName, string? englishName, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
