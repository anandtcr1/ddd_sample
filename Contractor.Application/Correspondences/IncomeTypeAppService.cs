using AutoMapper;
using Contractor.Tools;

namespace Contractor.Correspondences
{
    public class IncomeTypeAppService : IIncomeTypeAppService
    {
        private readonly IIncomeTypeRepository _repository;
        private readonly IMapper _mapper;

        public IncomeTypeAppService(IIncomeTypeRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<IncomeTypeDto> CreateAsync(IncomeTypeDto incomeTypeDto)
        {
            var incomeType = IncomeType.Create(incomeTypeDto.ArabicDescription, incomeTypeDto.EnglishDescription);

            await _repository.CreateAsync(incomeType);

            await _repository.SaveChangesAsync();

            return _mapper.Map<IncomeTypeDto>(incomeType);
        }

        public async Task<ListServiceModel<IncomeTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize)
        {
            var list = await _repository.GetAllAsync(arabicDescription, englishDescription, pageNumber, pageSize);

            return new ListServiceModel<IncomeTypeDto>(list.TotalCount, _mapper.Map<List<IncomeTypeDto>>(list.List));
        }

        public async Task<IncomeTypeDto> GetAsync(int id)
        {
            var model = await _repository.GetAsync(id);

            return _mapper.Map<IncomeTypeDto>(model);
        }

        public async Task<IncomeTypeDto> UpdateAsync(IncomeTypeDto incomeTypeDto)
        {
            var model = await _repository.GetAsync(incomeTypeDto.Id);

            model.Update(incomeTypeDto.ArabicDescription, incomeTypeDto.EnglishDescription);

            await _repository.SaveChangesAsync();

            return _mapper.Map<IncomeTypeDto>(model);
        }
    }
}
