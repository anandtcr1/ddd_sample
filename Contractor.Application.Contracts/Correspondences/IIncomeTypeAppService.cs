using Contractor.Tools;

namespace Contractor.Correspondences
{
    public interface IIncomeTypeAppService
    {
        Task<IncomeTypeDto> CreateAsync(IncomeTypeDto incomeTypeDto);

        Task<IncomeTypeDto> UpdateAsync(IncomeTypeDto incomeTypeDto);

        Task<IncomeTypeDto> GetAsync(int id);

        Task<ListServiceModel<IncomeTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize);
    }
}
