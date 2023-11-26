using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Identities
{
    public class GetSubUserListViewModel : GetListViewModel
    {
        public string? DisplayName { get; set; }

        public string? Email { get; set; }
         
        public string? PhoneNumber { get; set; }

        public UserStatus? Status { get; set; }

    }
    public class SubUserCreateViewModel
    { 
        public string DisplayName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmedPassword { get; set; } 
         
    }
    public class SubUserUpdateViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string DisplayName { get; set; } = null!;

    }

    public class SubUserUpdateClaimsViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        public List<string> ClaimValueList { get; set; } = null!;

    }

}