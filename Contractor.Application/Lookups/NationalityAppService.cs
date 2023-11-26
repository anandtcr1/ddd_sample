using AutoMapper;
using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public class NationalityAppService : INationalityAppService
    {
        private readonly INationalityRepository _repository;
        private readonly IMapper _mapper;

        public NationalityAppService(INationalityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<NationalityDto> CreateAsync(NationalityDto nationalityDto)
        {
            var nationality = Nationality.Create(nationalityDto.ArabicName, nationalityDto.EnglishName);

            await _repository.CreateAsync(nationality);

            await _repository.SaveChangesAsync();

            return _mapper.Map<NationalityDto>(nationality);
        }

        public async Task<ListRepositoryModel<NationalityDto>> GetAllAsync(string? arabicName, string? englishName, int pageNumber, int pageSize)
        {
            var result = await _repository.GetAllAsync(arabicName, englishName, pageNumber, pageSize);

            var list = _mapper.Map<List<NationalityDto>>(result.List);

            return new ListRepositoryModel<NationalityDto>(result.TotalCount, list);
        }

        public async Task<NationalityDto> GetAsync(int id)
        {
            var nationality = await _repository.GetAsync(id);

            return _mapper.Map<NationalityDto>(nationality);
        }

        public async Task<NationalityDto> UpdateAsync(NationalityDto nationalityDto)
        {
            var nationality = await _repository.GetAsync(nationalityDto.Id);

            nationality.Update(nationalityDto.ArabicName, nationalityDto.EnglishName);

            await _repository.SaveChangesAsync();

            return _mapper.Map<NationalityDto>(nationality);
        }
    }
}
