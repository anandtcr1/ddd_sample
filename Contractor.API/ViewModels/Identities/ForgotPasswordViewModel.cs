using System.ComponentModel.DataAnnotations;

namespace Contractor.Identities
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [Required]
        public string ResetPasswordUrl { get; set; } = null!;
    }
}
