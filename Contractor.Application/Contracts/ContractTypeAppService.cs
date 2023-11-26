using AutoMapper;
using Contractor.Tools;

namespace Contractor.Contracts
{
    public class ContractTypeAppService : IContractTypeAppService
    {
        public readonly IContractTypeRepository _repository;
        public readonly IMapper _mapper;

        public ContractTypeAppService(IContractTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContractTypeDto> CreateAsync(ContractTypeDto contractTypeDto)
        {
            var contractType = ContractType.Create(contractTypeDto.ArabicDescription, contractTypeDto.EnglishDescription);
            
            await _repository.CreateAsync(contractType);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ContractTypeDto>(contractType);
        }

        public async Task<ListServiceModel<ContractTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize)
        {
            var list = await _repository.GetAllAsync(arabicDescription, englishDescription, pageNumber, pageSize);

            return new ListServiceModel<ContractTypeDto>(list.TotalCount, _mapper.Map<List<ContractTypeDto>>(list.List));
        }

        
        public async Task<ContractTypeDto> GetAsync(int id)
        {
            var model = await _repository.GetAsync(id);

            return _mapper.Map<ContractTypeDto>(model);
        }

        public async Task<ContractTypeDto> UpdateAsync(ContractTypeDto contractTypeDto)
        {
            var model = await _repository.GetAsync(contractTypeDto.Id);

            model.Update(contractTypeDto.ArabicDescription, contractTypeDto.EnglishDescription);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ContractTypeDto>(model);
        }
    }
}
