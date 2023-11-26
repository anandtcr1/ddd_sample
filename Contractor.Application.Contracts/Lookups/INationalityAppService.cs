using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public interface INationalityAppService
    {
        Task<NationalityDto> GetAsync(int id);

        Task<NationalityDto> CreateAsync(NationalityDto nationalityDto);

        Task<NationalityDto> UpdateAsync(NationalityDto nationalityDto);

        Task<ListRepositoryModel<NationalityDto>> GetAllAsync(string? arabicName, string? englishName, int pageNumber, int pageSize);
    }
}
