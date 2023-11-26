using AutoMapper;
using Contractor.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IMapper _mapper;

        public UserController(IUserAppService userAppService, IMapper mapper)
        {
            _userAppService = userAppService;
            _mapper = mapper;
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userAppService.GetAsync(id);
            return Ok(user);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.UserManagment)]
        public async Task<IActionResult> GetList(GetUserListViewModel request)
        {
            var users = await _userAppService.GetListAsyc(request.DisplayName,
                                                        request.Email,
                                                        request.EmailConfirmed,
                                                        request.PhoneNumber,
                                                        request.PhoneNumberConfirmed,
                                                        request.LockoutEnabled,
                                                        request.Role,
                                                        request.PageNumber,
                                                        request.PageSize);

            return Ok(users);
        }

        [HttpPost("GetConsultantList")]
        [ClaimRequirement(PageNames.ConsultantList)]
        public async Task<IActionResult> GetConsultantList(GetConsultantListViewModel request)
        {
            var users = await _userAppService.GetConsultantListAsyc(request.DisplayName,
                                                        request.Email,
                                                        request.PhoneNumber,
                                                        request.PageNumber,
                                                        request.PageSize);

            return Ok(users);
        }

        [HttpPost("CreateUser")]
        [ClaimRequirement(PageNames.UserManagment)]
        public async Task<IActionResult> CreateAsync(UserCreateViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userDto = _mapper.Map<UserDto>(request);
            var user = await _userAppService.CreateAsync(userDto, request.UserPassword, request.RoleId, request.UserType, userId.Value);
            return Ok(user);
        }

        [HttpPost("UpdateUser")]
        [ClaimRequirement(PageNames.UserManagment)]
        public async Task<IActionResult> UpdateAsync(UserUpdateViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userDto = _mapper.Map<UserDto>(request);
            var user = await _userAppService.UpdateAsync(userDto, userId.Value);
            return Ok(user);
        }

        [HttpPost]
        [ClaimRequirement(PageNames.UserManagment)]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel request)
        {
            await _userAppService.ResetPasswordAsync(request.Email, request.Password);

            return Ok();
        }

        [HttpPost("RemoveUser/{userId}")]
        [ClaimRequirement(PageNames.UserManagment)]
        public async Task<IActionResult> RemoveUserAsync(string userId)
        {
            var loggedInUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _userAppService.RemoveUserAsync(userId, loggedInUserId.Value);
            return Ok(user);
        }

        [HttpPost("ActivateUser/{userId}")]
        [ClaimRequirement(PageNames.UserManagment)]
        public async Task<IActionResult> ActivateUserAsync(string userId)
        {
            var loggedInUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _userAppService.ActivateUserAsync(userId, loggedInUserId.Value);
            return Ok(user);
        }


        [HttpPost("GetContractorList")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> GetContractorList(GetContractorListViewModel request)
        {
            var users = await _userAppService.GetContractorListAsyc(request.DisplayName, 
                                                        request.PageNumber,
                                                        request.PageSize);

            return Ok(users);
        }


    }
}
