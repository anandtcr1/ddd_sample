using AutoMapper;
using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectTypeAppService : IProjectTypeAppService
    {
        private readonly IProjectTypeRepository _repository;
        public readonly IMapper _mapper;

        public ProjectTypeAppService(IProjectTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProjectTypeDto> CreateAsync(ProjectTypeDto projectTypeDto)
        {
            var projectType = ProjectType.Create(projectTypeDto.ArabicDescription, projectTypeDto.EnglishDescription);

            await _repository.CreateAsync(projectType);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ProjectTypeDto>(projectType);
        }

        public async Task<ListServiceModel<ProjectTypeDto>> GetAllAsync(string arabicDescription, string englishDescription, int pageNumber, int pageSize)
        {
            var list = await _repository.GetAllAsync(arabicDescription, englishDescription, pageNumber, pageSize);

            return new ListServiceModel<ProjectTypeDto>(list.TotalCount, _mapper.Map<List<ProjectTypeDto>>(list.List));
        }

        public async Task<ProjectTypeDto> GetAsync(int id)
        {
            var model = await _repository.GetAsync(id);

            return _mapper.Map<ProjectTypeDto>(model);
        }

        public async Task<ProjectTypeDto> UpdateAsync(ProjectTypeDto projectTypeDto)
        {
            var model = await _repository.GetAsync(projectTypeDto.Id);

            model.Update(projectTypeDto.ArabicDescription, projectTypeDto.EnglishDescription);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ProjectTypeDto>(model);
        }
    }
}
