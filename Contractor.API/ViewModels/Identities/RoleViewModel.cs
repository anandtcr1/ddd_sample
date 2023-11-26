using Contractor.GeneralViewModels;

namespace Contractor.Identities
{
    public class GetRoleListViewModel:GetListViewModel
    {
        public string? Name { get; set; }
        
        public bool? IsActive { get; set; }
        
    }

    public class RoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set;}
        
        public bool IsActive { get; set; }
    }

    public class UpdateRolePagesViewModel
    {
        public string RoleId { get; set; }

        public List<int> PageIds { get; set; }

    }

}
