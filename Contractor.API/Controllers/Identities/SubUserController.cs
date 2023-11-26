using AutoMapper;
using Contractor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubUserController : ControllerBase
    {
        private readonly ISubUserAppService _subUserAppService;
        private readonly IMapper _mapper;

        public SubUserController(ISubUserAppService subUserAppService, IMapper mapper)
        {
            _subUserAppService = subUserAppService;
            _mapper = mapper;
        }


        [HttpPost("CreateUser")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> CreateAsync(SubUserCreateViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userDto = _mapper.Map<SubUserDto>(request);
            var user = await _subUserAppService.CreateAsync(userDto, request.UserPassword, userId.Value);
            return Ok(user);
        }

        [HttpGet("GetUserRoleClaims")]
        public async Task<IActionResult> GetUserRoleClaims()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var role = await _subUserAppService.GetUserRoleClaims(userId!.Value);

            return Ok(role);
        }


        [HttpGet("GetUser/{id}")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _subUserAppService.GetAsync(id);      
            return Ok(user);
        }


        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> GetList(GetSubUserListViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier); 
            var users = await _subUserAppService.GetListAsyc(userId.Value , request.DisplayName,
                                                        request.Email, 
                                                        request.PhoneNumber,  
                                                        request.Status,
                                                        request.PageNumber,
                                                        request.PageSize);

            return Ok(users);
        }

     
     

        [HttpPost("UpdateUser")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> UpdateAsync(SubUserUpdateViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userDto = _mapper.Map<SubUserDto>(request);
            var user = await _subUserAppService.UpdateAsync(userDto, userId.Value);
            return Ok(user);
        }


        [HttpPost("UpdateSubUserClaims")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> UpdateSubUserClaims(SubUserUpdateClaimsViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var user = await _subUserAppService.UpdateSubUserClaims(userId!.Value, request.Id, request.ClaimValueList);
            return Ok(user);
        }

        [HttpPost("SuspendUser/{userId}")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> SuspendUserAsync(string userId)
        {
            var loggedInUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _subUserAppService.SuspendUserAsync(userId, loggedInUserId.Value);
            return Ok(user);
        }

        [HttpPost("RemoveUser/{userId}")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> RemoveUserAsync(string userId)
        {
            var loggedInUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _subUserAppService.RemoveUserAsync(userId, loggedInUserId.Value);
            return Ok(user);
        }

    }
}
