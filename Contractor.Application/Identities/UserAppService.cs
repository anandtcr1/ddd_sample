using AutoMapper;
using Contractor.Files;
using Contractor.GeneralEntities;
using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IBlobManager _blobManager;
        private readonly IRepository<Company, int> _companyRepository;

        public UserAppService(IUserRepository userRepository,
            IMapper mapper,
            IUserManager userManager,
            IRoleRepository roleRepository,
            IBlobManager blobManager,
            IRepository<Company, int> companyRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _blobManager = blobManager;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto> GetAsync(string id)
        {
            #region Call Repository
            var user = await _userRepository.GetByIdAsync(id);
            #endregion

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// GetListAsyc
        /// </summary>
        /// <param name="userSearchDto"></param>
        /// <returns></returns>
        public async Task<ListServiceModel<UserListDto>> GetListAsyc(string? displayName, 
                                                                    string? email, 
                                                                    bool? emailConfirmed, 
                                                                    string? phoneNumber, 
                                                                    bool? phoneNumberConfirmed, 
                                                                    bool? lockoutEnabled,
                                                                    string? role,
                                                                    int pageNumber,
                                                                    int pageSize)
        {
            #region Call Repository
            var users = await _userRepository.GetListAsyc(displayName, email, emailConfirmed, phoneNumber, phoneNumberConfirmed, lockoutEnabled, role, pageNumber, pageSize);
            #endregion

            var usersDto = _mapper.Map<List<UserListDto>>(users.List);
            return new ListServiceModel<UserListDto>(users.TotalCount, usersDto);
        }

        public async Task<ListServiceModel<UserListDto>> GetConsultantListAsyc(string? displayName,
                                                                    string? email,
                                                                    string? phoneNumber,
                                                                    int pageNumber,
                                                                    int pageSize)
        {
            #region Call Repository
            var users = await _userRepository.GetConsultantListAsyc(displayName, email, phoneNumber, pageNumber, pageSize);
            #endregion

            var usersDto = _mapper.Map<List<UserListDto>>(users.List);
            return new ListServiceModel<UserListDto>(users.TotalCount, usersDto);
        }

        
        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<UserDto> CreateAsync(UserDto userDto, string userPassword, string roleId, UserTypes userType, string insertUserId)
        {
            var company = Company.Create();

            await _companyRepository.AddAsync(company);

            var user = User.Create(userDto.DisplayName, userDto.Email, userDto.PhoneNumber, userType, company, insertUserId);           

            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            var result = await _userManager.CreateAsync(user, userPassword, role.Name);

            if(userType != UserTypes.Admin)
            {
                await _blobManager.CreateProfileMediaFolders(result.Id, result.Email);
            }

            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(result);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<UserDto> UpdateAsync(UserDto userDto, string lastModifyUserId)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);
            user.Update(userDto.DisplayName, lastModifyUserId);
            
            var result = await _userManager.UpdateAsync(user);
            return _mapper.Map<UserDto>(result);
        }

        public async Task ResetPasswordAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPassword(user, token, password);
        }

        public async Task<UserDto> RemoveUserAsync(string userId, string deleteUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.RemoveUser(deleteUserId);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> ActivateUserAsync(string userId, string lastModifiedUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.ActivateUser(lastModifiedUserId);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<ListServiceModel<UserListDto>> GetContractorListAsyc(string? displayName, int pageNumber, int pageSize)
        {
            #region Call Repository
            var users = await _userRepository.GetContractorListAsyc(displayName,  pageNumber, pageSize);
            #endregion

            var usersDto = _mapper.Map<List<UserListDto>>(users.List);
            return new ListServiceModel<UserListDto>(users.TotalCount, usersDto);
        }
    }
}
