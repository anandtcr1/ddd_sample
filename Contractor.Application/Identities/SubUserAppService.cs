using AutoMapper;
using Contractor.Exceptions;
using Contractor.Files;
using Contractor.Tools;

namespace Contractor.Identities
{
    public class SubUserAppService : ISubUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IBlobManager _blobManager;
        private readonly IPageRepository _pageRepository;


        public SubUserAppService(IUserRepository userRepository,
            IMapper mapper,
            IUserManager userManager,
            IRoleRepository roleRepository,
            IBlobManager blobManager,
            IPageRepository pageRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _blobManager = blobManager;
            _pageRepository = pageRepository;
        }



        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<SubUserDto> CreateAsync(SubUserDto userDto, string userPassword, string userId)
        {
            var parentUser = await _userRepository.GetByIdAsync(userId);
            UserTypes? usertype = null;

            switch (parentUser!.UserType)
            {
                case UserTypes.Owner:
                    usertype = UserTypes.SubOwner;
                    break;

                case UserTypes.Contractor:
                    usertype = UserTypes.SubContractor;
                    break;

                case UserTypes.Consultant:
                    usertype = UserTypes.SubConsultant;
                    break;
                default:
                    throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            var roleId = parentUser.Roles.Select(x => x.RoleId).FirstOrDefault();

            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            var user = User.Create(userDto.DisplayName, userDto.Email, userDto.PhoneNumber, usertype!.Value, parentUser.Company, userId);

            var result = await _userManager.CreateAsync(user, userPassword);

            result.AddPageClaims(role.Claims.Where(x => x.ClaimType == CustomClaimTypes.Page).Select(x => new Tuple<int, string>(x.PageId.Value, x.ClaimValue)).ToList());
            result.AddFunctionalityClaims(role.Claims.Where(x => x.ClaimType == CustomClaimTypes.Functionality).Select(x => x.ClaimValue).ToList());

            await _blobManager.CreateProfileMediaFolders(result.Id, result.Email);

            await _userRepository.SaveChangesAsync();

            return _mapper.Map<SubUserDto>(result);
        }


        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SubUserDto> GetAsync(string id)
        {
            #region Call Repository
            var user = await _userRepository.GetByIdAsync(id);

            var subUserDto= _mapper.Map<SubUserDto>(user);

            if (!string.IsNullOrEmpty(user.InsertUserId))
            {
                subUserDto.InsertUserName = (await _userRepository.GetByIdAsync(user.InsertUserId)).DisplayName;
                
            }
            #endregion

            return subUserDto;
        }

        /// <summary>
        /// GetListAsyc
        /// </summary>
        /// <param name="userSearchDto"></param>
        /// <returns></returns>
        public async Task<ListServiceModel<SubUserListDto>> GetListAsyc(string parentId, string? displayName,
                                                                    string? email,
                                                                    string? phoneNumber,
                                                                    UserStatus? status,
                                                                    int pageNumber,
                                                                    int pageSize)
        {
            #region Call Repository
            var parentUser = await _userRepository.GetByIdAsync(parentId);
            var users = await _userRepository.GetSubUserListAsyc(parentUser.CompanyId, displayName, email, phoneNumber, status, pageNumber, pageSize);
            #endregion

            var usersDto = _mapper.Map<List<SubUserListDto>>(users.List);
            return new ListServiceModel<SubUserListDto>(users.TotalCount, usersDto);
        }

        public async Task<List<RoleClaimDto>> GetUserRoleClaims(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var roleList = await _roleRepository.GetRoleByIdAsync(user!.Roles.Select(x => x.RoleId).ToList());

            return roleList.SelectMany(x => x.Claims.Select(y => new RoleClaimDto
            {
                Id = y.Id,
                RoleId = y.RoleId,
                ClaimType = y.ClaimType,
                ClaimValue = y.ClaimValue,
                PageId = y.PageId
            })).ToList();
        }

        public async Task<SubUserDto> RemoveUserAsync(string userId, string deleteUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.RemoveUser(deleteUserId);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<SubUserDto>(user);
        }

        public async Task<SubUserDto> SuspendUserAsync(string userId, string lastModifyUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.SuspendUser(lastModifyUserId);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<SubUserDto>(user);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<SubUserDto> UpdateAsync(SubUserDto userDto, string lastModifyUserId)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);
            user.Update(userDto.DisplayName, lastModifyUserId);

            var result = await _userManager.UpdateAsync(user);
            return _mapper.Map<SubUserDto>(result);
        }

        public async Task<SubUserDto> UpdateSubUserClaims(string parentId, string subUserId, List<string> claimValueList)
        {
            var parentUser = await _userRepository.GetByIdAsync(parentId);
            var subUser = await _userRepository.GetByIdAsync(subUserId);

            if (parentUser!.CompanyId != subUser!.CompanyId)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            if(claimValueList.Any(x=> !PageNames.AllPages.Contains(x) && !FunctionalityNames.AllFunctionalities.Contains(x)))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            var pageCalims = claimValueList.Where(PageNames.AllPages.Contains).ToList();
            var functionalityCalims = claimValueList.Where(FunctionalityNames.AllFunctionalities.Contains).ToList();

            var pages = await _pageRepository.GetByName(pageCalims);

            subUser.UpdatePageClaims(pages.Select(x => new Tuple<int, string>(x.Id, x.Name)).ToList());
            subUser.UpdateFunctionalityClaims(functionalityCalims);

            await _userRepository.SaveChangesAsync();


            return _mapper.Map<SubUserDto>(subUser);

        }



    }
}
