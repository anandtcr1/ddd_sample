using Contractor.Tenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class TendersListDto
    {
        public int Id { get; set; }

        public string ProjectNumber { get; set; } = null!;

        public string TenderNumber { get; set; } = null!;

        public string TenderTitle { get; set; } = null!;

        public DateTime  TenderDate { get; set; } 

        public DateTime OpenDate { get; set; }

        public DateTime EndDate { get; set; }

        public TenderStatus Status { get; set; }

        public TenderInvitationStatus? ContractorInvitationStatus { get; set; }

        public string Winner { get; set; }

    }
}
