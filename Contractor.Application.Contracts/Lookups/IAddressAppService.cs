using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public interface IAddressAppService
    {
        Task<CityDto> GetCityAsync(int id);

        Task<CityDto> CreateCityAsync(CityDto cityDto);

        Task<CityDto> UpdateCityAsync(CityDto cityDto);

        Task<ListServiceModel<CityDto>> GetAllCityAsync(string? arabicName, string? englishName, int pageNumber, int pageSize);

        Task<AreaDto> GetAreaAsync(int id);

        Task<AreaDto> CreateAreaAsync(AreaDto areaDto);

        Task<AreaDto> UpdateAreaAsync(AreaDto areaDto);

        Task<ListServiceModel<AreaDto>> GetAllAreaAsync(int cityId, string? arabicName, string? englishName, int pageNumber, int pageSize);
    }
}
