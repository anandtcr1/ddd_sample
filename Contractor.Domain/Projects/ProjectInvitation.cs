using Contractor.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectInvitation
    {
        private ProjectInvitation(string email, int projectId, ProjectInvitationStatus status, ProjectUserType userType)
        {
            Email = email;
            ProjectId = projectId;
            Status = status;
            UserType = userType;
        }


        [Key]
        public int Id { get; private set; }

        [Required]
        [EmailAddress]
        public string Email { get; private set; } = null!;

        public int ProjectId { get; private set; }

        public ProjectInvitationStatus Status { get; private set; }

        public ProjectUserType UserType { get; private set; }


        public virtual Project? Project { get; private set; }



        public static ProjectInvitation Create(string email, int projectId, ProjectInvitationStatus status, ProjectUserType userType)
        {
            return new ProjectInvitation(email, projectId, status, userType);
        }

        public void Accept()
        {
            if (Status != ProjectInvitationStatus.Pending)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            Status = ProjectInvitationStatus.Accept;
        }

        public void Decline()
        {
            if (Status != ProjectInvitationStatus.Pending)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            Status = ProjectInvitationStatus.Reject;

        }
    }
}
