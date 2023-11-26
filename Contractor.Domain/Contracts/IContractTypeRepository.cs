using Contractor.Tools;

namespace Contractor.Contracts
{
    public interface IContractTypeRepository
    {
        Task<ContractType> CreateAsync(ContractType contractType);
        Task<ContractType> GetAsync(int id);
        Task<List<ContractType>> GetAllAsync(string? search);
        Task<ListRepositoryModel<ContractType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();

    }
}
