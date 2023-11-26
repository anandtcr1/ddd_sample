using Contractor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateAppService _appService;

        public AuthenticateController(IAuthenticateAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        [Route("login")]        
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel model)
        {
            var credentials =  await _appService.GenerateCredentials(model.Email, model.Password);

            return Ok(credentials);
        }
        
        [HttpPost]
        [Route("loginAs")]
        [ClaimRequirement(FunctionalityNames.Impersonation)]
        public async Task<IActionResult> loginAs(string email)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var credentials = await _appService.GenerateCredentialsAs(userId.Value, email);

            return Ok(credentials);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel model)
        {
            var user = await _appService.RegisterUser(model.Password, model.DisplayName, model.Email, model.PhoneNumber, model.UserType);

            return Ok(user);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            await _appService.ForgotPassword(viewModel.Email, viewModel.ResetPasswordUrl);

            return Ok();
        }

        [HttpPost]
        [Route("ForgotPasswordConfirmation")]
        public async Task<IActionResult> ForgotPasswordConfirmation([FromBody] ForgotPasswordConfirmationViewModel viewModel)
        {
            await _appService.ForgotPasswordConfirmation(viewModel.Email, viewModel.Token, viewModel.Password);

            return Ok();
        }
    }
}
