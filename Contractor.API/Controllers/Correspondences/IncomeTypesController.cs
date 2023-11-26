using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Correspondences
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeTypesController : ControllerBase
    {
        private readonly IIncomeTypeAppService _appService;
        private readonly IMapper _mapper;


        public IncomeTypesController(IIncomeTypeAppService appService, 
            IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var incomeType = await _appService.GetAsync(id);

            return Ok(incomeType);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> GetListAsync(GetIncomeTypeListViewModel viewModel)
        {
            var list = await _appService.GetAllAsync(viewModel.ArabicDescription, viewModel.EnglishDescription, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> CreateAsync(IncomeTypeViewModel viewModel)
        {
            var request = _mapper.Map<IncomeTypeDto>(viewModel);

            var income = await _appService.CreateAsync(request);

            return Ok(income);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> UpdateAsync(IncomeTypeViewModel viewModel)
        {
            var request = _mapper.Map<IncomeTypeDto>(viewModel);

            var income = await _appService.UpdateAsync(request);

            return Ok(income);
        }
    }
}
