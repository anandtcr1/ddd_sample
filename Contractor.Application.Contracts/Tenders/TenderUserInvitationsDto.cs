using Contractor.Identities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class TenderUserInvitationsDto
    {
        public TenderUserInvitationsDto()
        { 
        }

        public int Id { get; set; }

        public int TenderId { get; set; }

        public string ContractorId { get; set; } = null!;
        public string ContractorName { get; set; } = null!;

        public TenderInvitationStatus Status { get; set; }

        public string? Notes { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public List<InvitationAccessDefinitionDto> InvitationAccessDefinitions { get; set; }

    }
}
