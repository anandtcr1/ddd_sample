using System.ComponentModel.DataAnnotations;

namespace Contractor.Identities
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
