using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public interface IAuthenticateAppService
    {
        Task<TokenResponse> GenerateCredentials(string email, string password);
        
        Task<TokenResponse> GenerateCredentialsAs(string ImpersonatorId, string email);
        
        Task<UserDto> RegisterUser(string password,string displayName, string email, string phoneNumber, UserTypes userType);

        Task ForgotPassword(string email, string resetPasswordUrl);

        Task ForgotPasswordConfirmation(string email, string token, string password);
    }
}
