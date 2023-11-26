using AutoMapper;
using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Correspondences
{
    public class OutGoingTypeAppService: IOutGoingTypeAppService
    {
        private readonly IOutGoingTypeRepository _repository;
        private readonly IMapper _mapper;

        public OutGoingTypeAppService(IOutGoingTypeRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<OutGoingTypeDto> CreateAsync(OutGoingTypeDto outGoingTypeDto)
        {
            var outType = OutGoingType.Create(outGoingTypeDto.ArabicDescription, outGoingTypeDto.EnglishDescription);

            await _repository.CreateAsync(outType);

            await _repository.SaveChangesAsync();

            return _mapper.Map<OutGoingTypeDto>(outType);
        }

        public async Task<ListServiceModel<OutGoingTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize)
        {
            var list = await _repository.GetAllAsync(arabicDescription, englishDescription, pageNumber, pageSize);

            return new ListServiceModel<OutGoingTypeDto>(list.TotalCount, _mapper.Map<List<OutGoingTypeDto>>(list.List));
        }

        public async Task<OutGoingTypeDto> GetAsync(int id)
        {
            var model = await _repository.GetAsync(id);

            return _mapper.Map<OutGoingTypeDto>(model);
        }

        public async Task<OutGoingTypeDto> UpdateAsync(OutGoingTypeDto outGoingTypeDto)
        {
            var model = await _repository.GetAsync(outGoingTypeDto.Id);

            model.Update(outGoingTypeDto.ArabicDescription, outGoingTypeDto.EnglishDescription);

            await _repository.SaveChangesAsync();

            return _mapper.Map<OutGoingTypeDto>(model);
        }
    }
}
