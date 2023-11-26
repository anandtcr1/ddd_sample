using Contractor.GeneralViewModels;
using Contractor.Identities;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Projects
{
    public class GetProjectListViewModel:GetListViewModel
    {
        public string? ProjectNumber { get; set; }

        public int? ProjectTypdId { get; set; }

        public string? OwnerName { get; set; }
        
        public string? ProjectTitle { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public string? Description { get; set; }
        
        public ProjectStatus? Status { get; set; }
        
    }

    public class ProjectViewModel
    {
        public int Id { get; set; }
        
        public string? OwnerId { get; set; }

        public int? DraftProjectId { get; set; }

        public int ProjectTypdId { get; set; }

        public string ProjectNumber { get; set; } = null!;

        public string ProjectTitle { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; } = null!;

        public ProjectStatus? Status { get; set; }
    }

    public class ProjectUserViewModel
    {
        public int ProjectId { get; set; }

        [Required]
        public List<string> UserIdList { get; set; } = null!;
    }

    public class ProjectInvitationViewModel
    {
        public int ProjectId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string RedirectUrl { get; set; } = null!;
    }
}
