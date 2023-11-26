using AutoMapper;
using Contractor.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IMapper _mapper;

        public RoleController(IRoleAppService roleAppService, IMapper mapper)
        {
            _roleAppService = roleAppService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.RoleManagment, PageNames.UserManagment)]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleAppService.GetAsync(id);

            return Ok(role);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.RoleManagment)]
        public async Task<IActionResult> GetList(GetRoleListViewModel request)
        {
            var response = await _roleAppService.GetListAsync(request.Name,
                request.IsActive,
                request.PageNumber, request.PageSize);

            return Ok(response);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.RoleManagment)]
        public async Task<IActionResult> Create(RoleViewModel request)
        {
            var roleDto = _mapper.Map<RoleDto>(request);

            var response = await _roleAppService.CreateAsync(roleDto);

            return Ok(response);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.RoleManagment)]
        public async Task<IActionResult> Update(RoleViewModel request)
        {
            var roleDto = _mapper.Map<RoleDto>(request);

            var response = await _roleAppService.UpdateAsync(roleDto);

            return Ok(response);
        }

        [HttpPost("UpdateRolePages")]
        [ClaimRequirement(PageNames.RoleManagment)]
        public async Task<IActionResult> UpdateRolePages(UpdateRolePagesViewModel request)
        {
            var response = await _roleAppService.UpdateRolePagesAsync(request.RoleId, request.PageIds);

            return Ok(response);
        }
    }
}
