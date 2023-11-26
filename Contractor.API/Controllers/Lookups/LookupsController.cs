using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Lookups
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupAppService _lookupAppService;
        
        public LookupsController(ILookupAppService lookupAppService)
        {
            _lookupAppService = lookupAppService;
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<Lookup<string>>>> GetRoles(string? search)
        {
            var list = await _lookupAppService.GetRoles(search);

            return Ok(list);
        }

        [Authorize]
        [HttpGet("GetProjects")]
        public async Task<ActionResult<List<Lookup<int>>>> GetProjects(string? search)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _lookupAppService.GetProjects(userId!.Value, search);

            return Ok(list);
        }

        [HttpGet("GetCities")]
        public async Task<ActionResult<List<Lookup<int>>>> GetCities(string? search)
        {
            var list = await _lookupAppService.GetCities(search);

            return Ok(list);
        }

        [HttpGet("GetAreas/{cityId}")]
        public async Task<ActionResult<List<Lookup<int>>>> GetAreas(int cityId,string? search)
        {
            var list = await _lookupAppService.GetAreas(cityId, search);

            return Ok(list);
        }

        [HttpGet("GetNationalities")]
        public async Task<ActionResult<List<Lookup<int>>>> GetNationalities(string? search)
        {
            var list = await _lookupAppService.GetNationalities(search);

            return Ok(list);
        }

        [HttpGet("GetContractTypes")]
        public async Task<ActionResult<List<Lookup<int>>>> GetContractTypes(string? search)
        {
            var list = await _lookupAppService.GetContractTypes(search);
            return Ok(list);
        }

        [HttpGet("GetOutGoingTypes")]
        public async Task<ActionResult<List<Lookup<int>>>> GetOutGoingTypes(string? search)
        {
            var list = await _lookupAppService.GetOutGoingTypes(search);
            return Ok(list);
        }

        [HttpGet("GetIncomeTypes")]
        public async Task<ActionResult<List<Lookup<int>>>> GetIncomeTypes(string? search)
        {
            var list = await _lookupAppService.GetIncomeTypes(search);
            return Ok(list);
        }

        [HttpGet("GetProjectTypes")]
        public async Task<ActionResult<List<Lookup<int>>>> GetProjectTypes(string? search)
        {
            var list = await _lookupAppService.GetProjectTypes(search);
            return Ok(list);
        }
    }
}
