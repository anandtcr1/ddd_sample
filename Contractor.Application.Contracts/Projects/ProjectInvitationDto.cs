
namespace Contractor.Projects
{
    public class ProjectInvitationDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public int ProjectId { get; set; }

        public ProjectInvitationStatus Status { get; set; }

        public ProjectUserType UserType { get; set; }
    }
}
