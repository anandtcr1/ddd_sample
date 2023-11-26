using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Contractor.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypesController : ControllerBase
    {
        private readonly IProjectTypeAppService _appService;
        private readonly IMapper _mapper;

        public ProjectTypesController(IProjectTypeAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var contractType = await _appService.GetAsync(id);

            return Ok(contractType);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> GetListAsync(GetProjectTypeListViewModel viewModel)
        {
            var list = await _appService.GetAllAsync(viewModel.ArabicDescription, viewModel.EnglishDescription, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> CreateAsync(ProjectTypeViewModel viewModel)
        {
            var request = _mapper.Map<ProjectTypeDto>(viewModel);

            var contractType = await _appService.CreateAsync(request);

            return Ok(contractType);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> UpdateAsync(ProjectTypeViewModel viewModel)
        {
            var request = _mapper.Map<ProjectTypeDto>(viewModel);

            var contractType = await _appService.UpdateAsync(request);

            return Ok(contractType);
        }
    }
}
