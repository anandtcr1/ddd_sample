using Contractor.Exceptions;
using Contractor.Identities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class DraftProject
    {

        private DraftProject(string? ownerId, string? consultantId, DateTime createdDate)
        {
            OwnerId = ownerId;
            ConsultantId = consultantId;
            StatusId = DraftProjectStatus.Pending;
            CreatedDate = createdDate;
        }

        [Key]
        public int Id { get; private set; }

        [ForeignKey("User")]
        public string OwnerId { get; private set; }

        [ForeignKey("User")]
        public string ConsultantId { get; private set; }
        public DraftProjectStatus StatusId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public virtual User? Owner { get; private set; }
        public virtual User? Consultant { get; private set; }
        public virtual Project? Project { get; private set; }

        public static DraftProject Create(string? ownerId, string? consultantId, DateTime createdDate)
        {
            return new DraftProject(ownerId, consultantId, createdDate);
        }

        public void UpdateAcceptStatus()
        {
            if (StatusId != DraftProjectStatus.Pending)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidStatusChange);
            }

            StatusId = DraftProjectStatus.Accept;
        }

        public void UpdateRejectStatus()
        {
            if (StatusId != DraftProjectStatus.Pending)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidStatusChange);
            }

            StatusId = DraftProjectStatus.Reject;
        }

        public void UpdateProjectCreatedStatus()
        {
            if(StatusId != DraftProjectStatus.Accept)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidStatusChange);
            }

            StatusId = DraftProjectStatus.ProjectCreated;
        }
    }
}
