using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IUserAppService
    {
        Task<UserDto> GetAsync(string id);
        
        Task<ListServiceModel<UserListDto>> GetListAsyc(string? displayName,
                                                        string? email, 
                                                        bool? emailConfirmed, 
                                                        string? phoneNumber, 
                                                        bool? phoneNumberConfirmed, 
                                                        bool? lockoutEnabled,
                                                        string? role,
                                                        int pageNumber, 
                                                        int pageSize);

        Task<ListServiceModel<UserListDto>> GetConsultantListAsyc(string? displayName,
                                                        string? email,
                                                        string? phoneNumber,
                                                        int pageNumber,
                                                        int pageSize); 
        
        Task<UserDto> CreateAsync(UserDto userDto, string userPassword, string roleId, UserTypes userType, string insertUserId);
        
        Task<UserDto> UpdateAsync(UserDto userDto, string lastModifyUserId);

        Task ResetPasswordAsync(string email, string password);

        Task<UserDto> RemoveUserAsync(string userId, string deleteUserId);

        Task<UserDto> ActivateUserAsync(string userId, string lastModifiedUserId);
        Task<ListServiceModel<UserListDto>> GetContractorListAsyc(string? displayName, int pageNumber, int pageSize);
    }
}
