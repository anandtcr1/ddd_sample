using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountManagementController : ControllerBase
    {
        private readonly IAccountManagementAppService _accountManagementAppService;
        private readonly IMapper _mapper;

        public AccountManagementController(IAccountManagementAppService accountManagementAppService, 
            IMapper mapper)
        {
            _accountManagementAppService = accountManagementAppService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetUserSettings")]
        public async Task<IActionResult> GetUserSettings()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var pages = await _accountManagementAppService.GetUserSettingsAsync(userId!.Value);

            return Ok(pages);
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var user = await _accountManagementAppService.GetUserAsync(userId.Value);

            return Ok(user);
        }

        [HttpPost]
        [Route("CreateProfile")]
        public async Task<IActionResult> CreateProfileAsync([FromForm] UserProfileViewModel userProfileViewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var request = _mapper.Map<UserProfileDto>(userProfileViewModel);

            request.UserId = userId.Value;

            var user = await _accountManagementAppService.CreateProfileAsync(request, userProfileViewModel.ProfilePicture, userProfileViewModel.ProfileCover, userProfileViewModel.Attachments);

            return Ok(user);
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfileAsync([FromForm] UserProfileViewModel userProfileViewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var request = _mapper.Map<UserProfileDto>(userProfileViewModel);

            request.UserId = userId.Value;
            
            var user = await _accountManagementAppService.UpdateProfileAsync(request, userProfileViewModel.ProfilePicture, userProfileViewModel.ProfileCover, userProfileViewModel.Attachments);

            return Ok(user);
        }

        [HttpPost]
        [Route("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePictureAsync(IFormFile ProfilePicture)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var user = await _accountManagementAppService.UpdateProfilePictureAsync(userId.Value, ProfilePicture);

            return Ok(user);
        }

        [HttpPost]
        [Route("UpdateProfileCover")]
        public async Task<IActionResult> UpdateProfileCoverAsync(IFormFile ProfilePicture)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var user = await _accountManagementAppService.UpdateProfileCoverAsync(userId.Value, ProfilePicture);

            return Ok(user);
        }
    }
}
