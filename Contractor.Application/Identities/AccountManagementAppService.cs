using AutoMapper;
using Contractor.Exceptions;
using Contractor.Files;
using Contractor.Tools.Memory;
using Microsoft.AspNetCore.Http;

namespace Contractor.Identities
{
    public class AccountManagementAppService : IAccountManagementAppService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IBlobManager _blobManager;
        private readonly IMemoryCacheManager _memoryCacheManager;

        public AccountManagementAppService(IPageRepository pageRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IFileRepository fileRepository,
            IBlobManager blobManager,
            IMemoryCacheManager memoryCacheManager)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _blobManager = blobManager;
            _memoryCacheManager = memoryCacheManager;
        }


        public async Task<GetUserSettingsResponse> GetUserSettingsAsync(string userId)
        {
            var returnedModel = new GetUserSettingsResponse();
            var user = await _userRepository.GetByIdAsync(userId);
            
            var pagesName = user.Roles.SelectMany(x=>x.Role.Claims.Where(y=>y.ClaimType == CustomClaimTypes.Page)).Select(x=>x.ClaimValue).ToList();

            pagesName.AddRange(user.Claims.Where(x => x.ClaimType == CustomClaimTypes.Page).Select(x => x.ClaimValue).ToList());

            var pages = await _pageRepository.GetByName(pagesName.Distinct().ToList());


            var functionsName = user.Roles.SelectMany(x => x.Role.Claims.Where(y => y.ClaimType == CustomClaimTypes.Functionality)).Select(x => x.ClaimValue).ToList();

            functionsName.AddRange(user.Claims.Where(x => x.ClaimType == CustomClaimTypes.Functionality).Select(x => x.ClaimValue).ToList());

            var dataUsage = await _blobManager.GetFolderSize(user.Email);

            returnedModel.StorageSpace = user.SubscriptionPlan?.StorageSpace ?? 0;
            returnedModel.DataUsage = dataUsage;

            returnedModel.Pages = _mapper.Map<List<PageDto>>(pages);
            returnedModel.Functions = functionsName;

            returnedModel.UserType = user.UserType;
            returnedModel.UserId = userId;

            return returnedModel;
        }

        public async Task<UserDto> CreateProfileAsync(UserProfileDto userProfileDto, IFormFile? ProfilePicture, IFormFile? ProfileCover, List<IFormFile> Attachments)
        {
            var user = await _userRepository.GetByIdAsync(userProfileDto.UserId);

            user.CreateProfile(userProfileDto.FirstName,
                userProfileDto.LastName,
                userProfileDto.BirthDate,
                userProfileDto.Address,
                userProfileDto.Gender,
                userProfileDto.AlternativeEmail,
                userProfileDto.AreaId,
                userProfileDto.NationalityId);

            if (ProfilePicture != null)
            {
                AccessDefinition accessDefinition = await _blobManager.UploadProfileMediaFiles(user.Id, user.Email, ProfilePicture);

                (_, ProfileAccessDefinition profileNewPictureAccessDefinition) =user.ChangeProfilePicture();
                
                accessDefinition.AddProfileAccessDefinition(profileNewPictureAccessDefinition);
            }

            if (ProfileCover != null)
            {
                AccessDefinition accessDefinition = await _blobManager.UploadProfileMediaFiles(user.Id, user.Email, ProfileCover);

                (_, ProfileAccessDefinition profileNewCoverAccessDefinition) = user.ChangeProfileCover();

                accessDefinition.AddProfileAccessDefinition(profileNewCoverAccessDefinition);
            }

            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);

        }

        public async Task<UserDto> UpdateProfileAsync(UserProfileDto userProfileDto, IFormFile? ProfilePicture, IFormFile? ProfileCover, List<IFormFile> Attachments)
        {
            var user = await _userRepository.GetByIdAsync(userProfileDto.UserId);

            if (user.Profile == null)
            {
                throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(UserProfile), userProfileDto.UserId);
            }

            user.UpdateProfile(userProfileDto.FirstName,
                userProfileDto.LastName,
                userProfileDto.BirthDate,
                userProfileDto.Address,
                userProfileDto.Gender,
                userProfileDto.AlternativeEmail,
                userProfileDto.AreaId,
                userProfileDto.NationalityId);

            if(ProfilePicture != null)
            {
                AccessDefinition accessDefinition = await _blobManager.UploadProfileMediaFiles(user.Id,user.Email, ProfilePicture);

                (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewPictureAccessDefinition) = user.ChangeProfilePicture();

                accessDefinition.AddProfileAccessDefinition(profileNewPictureAccessDefinition);

                if (oldAccessDefinitionId.HasValue)
                    await _blobManager.Delete(oldAccessDefinitionId.Value, user.Id, true, true);
            }

            if (ProfileCover != null)
            {
                AccessDefinition accessDefinition = await _blobManager.UploadProfileMediaFiles(user.Id, user.Email, ProfileCover);

                (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewCoverAccessDefinition) = user.ChangeProfileCover();

                accessDefinition.AddProfileAccessDefinition(profileNewCoverAccessDefinition);

                if (oldAccessDefinitionId.HasValue)
                    await _blobManager.Delete(oldAccessDefinitionId.Value, user.Id, true, true);
            }


            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);

        }

        public async Task<UserDto> GetUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            return _mapper.Map<UserDto>(user);
        }


        public async Task<AccessDefinitionDto> UpdateProfilePictureAsync(string userId, IFormFile profilePicture)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user.Profile == null)
            {
                throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(UserProfile), userId);
            }

            AccessDefinition accessDefinition = await _blobManager.UploadProfileMediaFiles(userId, user.Email, profilePicture);

            (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewPictureAccessDefinition) = user.ChangeProfilePicture();

            accessDefinition.AddProfileAccessDefinition(profileNewPictureAccessDefinition);

            if (oldAccessDefinitionId.HasValue)
                await _blobManager.Delete(oldAccessDefinitionId.Value, user.Id, true, true);

            await _fileRepository.SaveChangesAsync();

            return _mapper.Map<AccessDefinitionDto>(accessDefinition);
        }

        public async Task<AccessDefinitionDto> UpdateProfileCoverAsync(string userId, IFormFile profileCover)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user.Profile == null)
            {
                throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(UserProfile), userId);
            }

            AccessDefinition accessDefinition = await _blobManager.UploadProfileMediaFiles(userId, user.Email, profileCover);

            (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewCoverAccessDefinition) = user.ChangeProfileCover();

            accessDefinition.AddProfileAccessDefinition(profileNewCoverAccessDefinition);

            if (oldAccessDefinitionId.HasValue)
                await _blobManager.Delete(oldAccessDefinitionId.Value, user.Id, true, true);

            await _fileRepository.SaveChangesAsync();

            return _mapper.Map<AccessDefinitionDto>(accessDefinition);
        }
    }
}
