using System.ComponentModel.DataAnnotations;

namespace Contractor.Identities
{
    public class ForgotPasswordConfirmationViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
