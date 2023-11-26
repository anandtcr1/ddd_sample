using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        
        Task<User> GetByIdAsync(string id);
        
        Task<List<User>> GetByIdAsync(List<string> idList);

        Task<List<User>> GetCompanyUsersAsync(int companyId);

        Task<bool> IsPhoneNumberExist(string phoneNumber);

        Task<ListRepositoryModel<User>> GetListAsyc(string? displayName,
                                                    string? email,
                                                    bool? emailConfirmed,
                                                    string? phoneNumber,
                                                    bool? phoneNumberConfirmed,
                                                    bool? lockoutEnabled,
                                                    string? role,
                                                    int pageNumber,
                                                    int pageSize);
        Task<ListRepositoryModel<User>> GetConsultantListAsyc(string? displayName,
                                                    string? email,
                                                    string? phoneNumber,
                                                    int pageNumber,
                                                    int pageSize); 

        Task<int> SaveChangesAsync();
        Task<ListRepositoryModel<User>> GetSubUserListAsyc( int companyId, 
                                                                string? displayName,
                                                                string? email, 
                                                                string? phoneNumber,
                                                                UserStatus? status,
                                                                int pageNumber, 
                                                                int pageSize);
        Task<ListRepositoryModel<User>> GetContractorListAsyc(string? displayName, int pageNumber, int pageSize);
    }
}
