using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public enum DraftProjectStatus
    {
        Pending,
        Accept,
        Reject,
        ProjectCreated
    }

    public enum ProjectUserType
    {
        Consultant,
        Contractor,
        Admin,
        Owner,
        SubOwner,
        SubConsultant,
        SubContractor
    }

    public enum ProjectInvitationStatus
    {
        Pending,
        Accept,
        Reject,
    }

    public enum ProjectStatus
    {
        Canceled,
        Active,
    }
}
