
using Microsoft.AspNetCore.Identity;

namespace Contractor.Identities
{
    public interface IUserManager
    {
        Task ResetPassword(User user, string token, string newPassword);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<bool> CheckPasswordAsync(User user, string password);

        Task<IList<string>> GetRolesAsync(User user);

        Task<User> CreateAsync(User user, string password, string? roleName = null);

        Task<User> UpdateAsync(User user);

        Task<IdentityResult> AddToRoleAsync(User user, string role);
    }
}
