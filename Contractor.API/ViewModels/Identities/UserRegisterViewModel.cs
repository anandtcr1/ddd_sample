using System.ComponentModel.DataAnnotations;

namespace Contractor.Identities
{
    public class UserRegisterViewModel
    {

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; } = null!;
        public UserTypes UserType { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")] 
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; } = null!;
    }
}
