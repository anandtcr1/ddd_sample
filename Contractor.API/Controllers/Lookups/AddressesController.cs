using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Lookups
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressAppService _appService;
        private readonly IMapper _mapper;

        public AddressesController(IMapper mapper, IAddressAppService addressAppService)
        {
            _mapper = mapper;
            _appService = addressAppService;
        }

        [HttpGet("GetCity/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var city = await _appService.GetCityAsync(id);

            return Ok(city);
        }

        [HttpPost("GetCityList")]
        public async Task<IActionResult> GetCityListAsync(GetCityListViewModel viewModel)
        {
            var list = await _appService.GetAllCityAsync(viewModel.ArabicName, viewModel.EnglishName, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateAsync(CityViewModel viewModel)
        {
            var request = _mapper.Map<CityDto>(viewModel);

            var city = await _appService.CreateCityAsync(request);

            return Ok(city);
        }

        [HttpPost("UpdateCity")]
        public async Task<IActionResult> UpdateAsync(CityViewModel viewModel)
        {
            var request = _mapper.Map<CityDto>(viewModel);

            var city = await _appService.UpdateCityAsync(request);

            return Ok(city);
        }



        [HttpGet("GetArea/{id}")]
        public async Task<IActionResult> GetAreaAsync(int id)
        {
            var outGoingCorrespondenceType = await _appService.GetAreaAsync(id);

            return Ok(outGoingCorrespondenceType);
        }

        [HttpPost("GetAreaList")]
        public async Task<IActionResult> GetAreaListAsync(GetAreaListViewModel viewModel)
        {
            var list = await _appService.GetAllAreaAsync(viewModel.CityId, viewModel.ArabicName, viewModel.EnglishName, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("CreateArea")]
        public async Task<IActionResult> CreateAreaAsync(AreaViewModel viewModel)
        {
            var request = _mapper.Map<AreaDto>(viewModel);

            var area = await _appService.CreateAreaAsync(request);

            return Ok(area);
        }

        [HttpPost("UpdateArea")]
        public async Task<IActionResult> UpdateAreaAsync(AreaViewModel viewModel)
        {
            var request = _mapper.Map<AreaDto>(viewModel);

            var area = await _appService.UpdateAreaAsync(request);

            return Ok(area);
        }
    }
}
