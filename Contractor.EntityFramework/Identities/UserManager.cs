using Contractor.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace Contractor.Identities
{

    public class UserManager : UserManager<User>, IUserManager
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IUserRepository userRepository)
            : base(store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
            _userRepository = userRepository;
        }

        public async Task ResetPassword(User user, string token, string newPassword)
        {
            var resetPasswordResult = await base.ResetPasswordAsync(user, token, newPassword);

            if (!resetPasswordResult.Succeeded)
            {
                List<IdentityError>  errorList = resetPasswordResult.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));

                throw new CustomValidationException(nameof(User), errors);

            }

        }

        public override Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return base.GeneratePasswordResetTokenAsync(user);
        }

        public override string? GetUserId(ClaimsPrincipal principal)
        {          
            return base.GetUserId(principal);
        }

        public override Task<string> GetUserIdAsync(User user)
        {
            return base.GetUserIdAsync(user);
        }

        public override Task<IList<string>> GetRolesAsync(User user)
        {
            return base.GetRolesAsync(user);
        }

        public override Task<bool> CheckPasswordAsync(User user, string password)
        {
            return base.CheckPasswordAsync(user, password);
        }

        async Task<User> IUserManager.CreateAsync(User user, string password,string? roleName = null)
        {
            List<IdentityError> errorList;

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                var isPhoneNumberExist = await _userRepository.IsPhoneNumberExist(user.PhoneNumber);

                if (isPhoneNumberExist)
                {
                    throw new CustomValidationException(nameof(User), string.Format(CustomValidationExceptionErrorMessages.TakenPhoneNumber, user.PhoneNumber));
                }
            }

            IdentityResult createUserResult = await base.CreateAsync(user, password);
            
            if (!createUserResult.Succeeded)
            {
                errorList = createUserResult.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));

                throw new CustomValidationException(nameof(User), errors);
                
            }

            if (!string.IsNullOrEmpty(roleName))
            {
                var adduserToRoleResult = await base.AddToRoleAsync(user, roleName);

                if (!adduserToRoleResult.Succeeded)
                {
                    errorList = adduserToRoleResult.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));

                    throw new CustomValidationException(nameof(User), errors);
                }
            }

            return user;
        }

        async Task<User> IUserManager.UpdateAsync(User user)
        {
            await base.UpdateAsync(user);
            return user;
        }
    }
}
