using Contractor.GeneralViewModels;

namespace Contractor.Identities
{
    public class GetUserListViewModel: GetListViewModel
    {
        public string? DisplayName { get; set; }
        
        public string? Email { get; set; }
        
        public bool? EmailConfirmed { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public bool? PhoneNumberConfirmed { get; set; }
        
        public bool? LockoutEnabled { get; set; }

        public string? Role { get; set; }

    }

    public class GetConsultantListViewModel : GetListViewModel
    {
        public string? DisplayName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

    }

    public class UserCreateViewModel
    {
        
        public string DisplayName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string UserPassword { get; set; }
        
        public string RoleId { get; set; }

        public UserTypes UserType { get; set; }
    }

    public class UserUpdateViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }


    public class GetContractorListViewModel : GetListViewModel
    {
        public string? DisplayName { get; set; }  
    }

}