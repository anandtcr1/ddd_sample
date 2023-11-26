using Contractor.Tools;

namespace Contractor.Correspondences
{
    public interface IOutGoingTypeAppService
    {
        Task<OutGoingTypeDto> CreateAsync(OutGoingTypeDto outGoingTypeDto);

        Task<OutGoingTypeDto> UpdateAsync(OutGoingTypeDto outGoingTypeDto);

        Task<OutGoingTypeDto> GetAsync(int id);

        Task<ListServiceModel<OutGoingTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize);
    }
}
