using Contractor.Tools;

namespace Contractor.Correspondences
{
    public interface IIncomeTypeRepository
    {
        Task<IncomeType> CreateAsync(IncomeType incomeType);

        Task<IncomeType> GetAsync(int id);

        Task<List<IncomeType>> GetAllAsync(string? search);
        
        Task<ListRepositoryModel<IncomeType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
