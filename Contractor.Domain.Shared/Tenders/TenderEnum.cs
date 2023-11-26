using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public enum TenderStatus
    {
        Draft,
        Active,
        Closed,
        Completed
    }

    public enum TenderInvitationStatus
    {
        Pending,
        Accepted,
        Rejected,
        Submitted,
        Winner,
    }
}
