using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Correspondences
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutGoingTypesController : ControllerBase
    {
        private readonly IOutGoingTypeAppService _appService;
        private readonly IMapper _mapper;


        public OutGoingTypesController(IOutGoingTypeAppService appService,
            IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var outGoingCorrespondenceType = await _appService.GetAsync(id);

            return Ok(outGoingCorrespondenceType);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> GetListAsync(GetOutGoingTypeListViewModel viewModel)
        {
            var list = await _appService.GetAllAsync(viewModel.ArabicDescription, viewModel.EnglishDescription, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> CreateAsync(OutGoingTypeViewModel viewModel)
        {
            var request = _mapper.Map<OutGoingTypeDto>(viewModel);

            var outGoing = await _appService.CreateAsync(request);

            return Ok(outGoing);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> UpdateAsync(OutGoingTypeViewModel viewModel)
        {
            var request = _mapper.Map<OutGoingTypeDto>(viewModel);

            var outGoing = await _appService.UpdateAsync(request);

            return Ok(outGoing);
        }
    }
}
