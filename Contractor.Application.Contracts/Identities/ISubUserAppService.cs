using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface ISubUserAppService

    {

        Task<SubUserDto> CreateAsync(SubUserDto userDto, string userPassword, string userId);
         
        Task<SubUserDto> GetAsync(string id);
        
        Task<ListServiceModel<SubUserListDto>> GetListAsyc(string parentId , string? displayName,
                                                        string? email,  
                                                        string? phoneNumber,
                                                        UserStatus? status,
                                                        int pageNumber, 
                                                        int pageSize);


        Task<List<RoleClaimDto>> GetUserRoleClaims(string userId);

        Task<SubUserDto> UpdateAsync(SubUserDto userDto, string lastModifyUserId);

        Task<SubUserDto> UpdateSubUserClaims(string parentId, string subUserId, List<string> claimValueList);

        Task<SubUserDto> SuspendUserAsync(string userId, string lastModifyUserId);

        Task<SubUserDto> RemoveUserAsync(string userId, string deleteUserId);


    }
}
