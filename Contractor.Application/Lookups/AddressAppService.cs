using AutoMapper;
using Contractor.Tools;

namespace Contractor.Lookups
{
    public class AddressAppService : IAddressAppService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressAppService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }



        public async Task<AreaDto> CreateAreaAsync(AreaDto areaDto)
        {
            var area = Area.Create(areaDto.ArabicName, areaDto.EnglishName, areaDto.CityId);

            await _addressRepository.CreateAreaAsync(area);

            await _addressRepository.SaveChangesAsync();

            return _mapper.Map<AreaDto>(area);
        }

        public async Task<CityDto> CreateCityAsync(CityDto cityDto)
        {
            var city = City.Create(cityDto.ArabicName, cityDto.EnglishName);

            await _addressRepository.CreateCityAsync(city);

            await _addressRepository.SaveChangesAsync();

            return _mapper.Map<CityDto>(city);
        }

        public async Task<ListServiceModel<AreaDto>> GetAllAreaAsync(int cityId, string? arabicName, string? englishName, int pageNumber, int pageSize)
        {
            var result = await _addressRepository.GetAllAreaAsync(cityId, arabicName, englishName, pageNumber, pageSize);

            var list = _mapper.Map<List<AreaDto>>(result.List);

            return new ListServiceModel<AreaDto>(result.TotalCount, list);
        }

        public async Task<ListServiceModel<CityDto>> GetAllCityAsync(string? arabicName, string? englishName, int pageNumber, int pageSize)
        {
            var result = await _addressRepository.GetAllCityAsync(arabicName, englishName, pageNumber, pageSize);

            var list = _mapper.Map<List<CityDto>>(result.List);

            return new ListServiceModel<CityDto>(result.TotalCount, list);
        }

        public async Task<AreaDto> GetAreaAsync(int id)
        {
            var areaDto = await _addressRepository.GetAreaAsync(id);

            return _mapper.Map<AreaDto>(areaDto);
        }

        public async Task<CityDto> GetCityAsync(int id)
        {
            var ctiyDto = await _addressRepository.GetCityAsync(id);

            return _mapper.Map<CityDto>(ctiyDto);
        }

        public async Task<AreaDto> UpdateAreaAsync(AreaDto areaDto)
        {
            var area = await _addressRepository.GetAreaAsync(areaDto.Id);

            area.Update(areaDto.ArabicName, areaDto.EnglishName, areaDto.CityId);

            await _addressRepository.SaveChangesAsync();

            return _mapper.Map<AreaDto>(area);
        }

        public async Task<CityDto> UpdateCityAsync(CityDto cityDto)
        {
            var city = await _addressRepository.GetCityAsync(cityDto.Id);

            city.Update(cityDto.ArabicName, cityDto.EnglishName);

            await _addressRepository.SaveChangesAsync();

            return _mapper.Map<CityDto>(city);
        }
    }
}
