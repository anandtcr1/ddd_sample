using AutoMapper;
using Contractor.Contracts;
using Contractor.Filters;
using Contractor.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Controllers.Contracts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractTypesController : ControllerBase
    {
        private readonly IContractTypeAppService _appService;
        private readonly IMapper _mapper;

        public ContractTypesController(IContractTypeAppService appService, IMapper mapper)
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
        public async Task<IActionResult> GetListAsync(GetContractTypeListViewModel viewModel)
        {
            var list = await _appService.GetAllAsync(viewModel.ArabicDescription, viewModel.EnglishDescription, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> CreateAsync(ContractTypeViewModel viewModel)
        {
            var request = _mapper.Map<ContractTypeDto>(viewModel);

            var contractType = await _appService.CreateAsync(request);

            return Ok(contractType);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.DefinitionFiles)]
        public async Task<IActionResult> UpdateAsync(ContractTypeViewModel viewModel)
        {
            var request = _mapper.Map<ContractTypeDto>(viewModel);

            var contractType = await _appService.UpdateAsync(request);

            return Ok(contractType);
        }

    }
}
