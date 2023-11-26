using AutoMapper;
using Contractor.Exceptions;
using Contractor.Files;
using Contractor.GeneralEntities;
using Contractor.Subscriptions;
using Contractor.Tools;
using Contractor.Tools.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Contractor.Identities
{
    public class AuthenticateAppService : IAuthenticateAppService
    {
        private readonly IMapper _mapper;
        private readonly IEmailManager _emailManager;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IBlobManager _blobManager;
        private readonly JWTSettings _jWTSettings;
        private readonly FrontEndUrls _frontEndUrls;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IRepository<Company, int> _companyRepository;

        public AuthenticateAppService(
            IEmailManager emailManager,
            IUserManager userManager,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IOptions<JWTSettings> jWTSettings,
            IOptions<FrontEndUrls> frontEndUrls,
            IMapper mapper,
            IBlobManager blobManager,
            ISubscriptionRepository subscriptionRepository,
            IRepository<Company, int> companyRepository)
        {
            _emailManager = emailManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jWTSettings = jWTSettings.Value;
            _frontEndUrls = frontEndUrls.Value;
            _mapper = mapper;
            _blobManager = blobManager;
            _subscriptionRepository = subscriptionRepository;
            _companyRepository = companyRepository;
        }

        public async Task<TokenResponse> GenerateCredentials(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (!(await _userManager.CheckPasswordAsync(user, password)))
            {
                throw new EntityNotFoundException(AuthenticationExceptionConstants.UserNotFound, nameof(User), email);
            }

            if(user.Status != UserStatus.Active)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.UserLockout);
            }

            if (await _userManager.CheckPasswordAsync(user, password))
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(CustomClaimTypes.UserType, user.UserType.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };

                var userRoles = await _userManager.GetRolesAsync(user);

                var roles = await _roleRepository.GetAsync(userRoles.ToList());

                foreach (var role in roles)
                {
                    authClaims.AddRange(role.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)));
                }

                if (user.Claims != null) 
                { 
                    authClaims.AddRange(user.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue))); 
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret));

                var token = new JwtSecurityToken(
                    issuer: _jWTSettings.ValidIssuer,
                    audience: _jWTSettings.ValidAudience,
                    expires: DateTime.Now.AddHours(_jWTSettings.Expires),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new TokenResponse
                {
                    Expiration = token.ValidTo,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }

            return null;
        }


        public async Task<TokenResponse> GenerateCredentialsAs(string ImpersonatorId, string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new EntityNotFoundException(AuthenticationExceptionConstants.UserNotFound, nameof(User), email);
            }

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(CustomClaimTypes.Impersonation, true.ToString()),
                    new Claim(CustomClaimTypes.ImpersonatorId, ImpersonatorId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleRepository.GetAsync(userRoles.ToList());

            foreach (var role in roles)
            {
                authClaims.AddRange(role.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jWTSettings.ValidIssuer,
                audience: _jWTSettings.ValidAudience,
                expires: DateTime.Now.AddHours(_jWTSettings.Expires),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new TokenResponse
            {
                Expiration = token.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<UserDto> RegisterUser(string password, string displayName, string email, string phoneNumber, UserTypes userType)
        {
            string? userRole = RoleNames.Contractor;

            switch (userType)
            {
                case UserTypes.Contractor:
                    userRole = RoleNames.Contractor;
                    break;
                case UserTypes.Consultant:
                    userRole = RoleNames.Consultant;
                    break;
                case UserTypes.Owner:
                    userRole = RoleNames.Owner;
                    break;
                case UserTypes.SubOwner:
                case UserTypes.SubConsultant:
                case UserTypes.SubContractor:
                case UserTypes.Admin:
                    throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidUserType);
            }

            var company = Company.Create();

            await _companyRepository.AddAsync(company);

            var user = User.Create(displayName, email, phoneNumber, userType, company, string.Empty);

            var subscription = await _subscriptionRepository.GetDefaultAsync();
            
            user.UpdateSubscriptionPlanId(subscription.Id);

            var result = await _userManager.CreateAsync(user, password, userRole);

            await _blobManager.CreateProfileMediaFolders(result.Id, result.Email);

            await _blobManager.CreateProfileChatFolders(result.Id, result.Email);

            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(result);
        }

        public async Task ForgotPassword(string email, string resetPasswordUrl)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = resetPasswordUrl + "&email=" + email + "&token=" + token;
            link = string.Format("<a href='{0}'>Reset password</a>", link);
            var message = Message.Create(new string[] { email }, "Reset Password", link);

            await _emailManager.SendEmail(message);
        }

        public async Task ForgotPasswordConfirmation(string email, string token, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            await _userManager.ResetPassword(user, token, password);
        }
    }
}
