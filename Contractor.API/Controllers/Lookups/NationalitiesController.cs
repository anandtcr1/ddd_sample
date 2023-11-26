using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Lookups
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NationalitiesController : ControllerBase
    {
        private readonly INationalityAppService _appService;
        private readonly IMapper _mapper;

        public NationalitiesController(IMapper mapper, INationalityAppService appService)
        {
            _mapper = mapper;
            _appService = appService;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var nationality = await _appService.GetAsync(id);

            return Ok(nationality);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetListAsync(GetNationalityListViewModel viewModel)
        {
            var list = await _appService.GetAllAsync(viewModel.ArabicName, viewModel.EnglishName, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(NationalityViewModel viewModel)
        {
            var request = _mapper.Map<NationalityDto>(viewModel);

            var nationality = await _appService.CreateAsync(request);

            return Ok(nationality);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(NationalityViewModel viewModel)
        {
            var request = _mapper.Map<NationalityDto>(viewModel);

            var nationality = await _appService.UpdateAsync(request);

            return Ok(nationality);
        }
    }
}
