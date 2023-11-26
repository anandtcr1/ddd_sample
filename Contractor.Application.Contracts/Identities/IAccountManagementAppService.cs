
using Contractor.Files;
using Microsoft.AspNetCore.Http;

namespace Contractor.Identities
{
    public interface IAccountManagementAppService
    {
        Task<GetUserSettingsResponse> GetUserSettingsAsync(string userId);

        Task<UserDto> CreateProfileAsync(UserProfileDto userProfileDto, IFormFile? ProfilePicture, IFormFile? ProfileCover, List<IFormFile> Attachments);

        Task<UserDto> UpdateProfileAsync(UserProfileDto userProfileDto, IFormFile? ProfilePicture, IFormFile? ProfileCover, List<IFormFile> Attachments);
        
        Task<AccessDefinitionDto> UpdateProfilePictureAsync(string userId, IFormFile profilePicture);
        
        Task<AccessDefinitionDto> UpdateProfileCoverAsync(string userId, IFormFile profileCover);
        
        Task<UserDto> GetUserAsync(string userId);
    }
}
