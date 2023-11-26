using Contractor.Tools;

namespace Contractor.Contracts
{
    public interface IContractTypeAppService
    {
        Task<ContractTypeDto> CreateAsync(ContractTypeDto contractTypeDto);

        Task<ContractTypeDto> UpdateAsync(ContractTypeDto contractTypeDto);

        Task<ContractTypeDto> GetAsync(int id);

        Task<ListServiceModel<ContractTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize);
    }
}
