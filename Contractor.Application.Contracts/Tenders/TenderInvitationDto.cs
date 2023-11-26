using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class TenderInvitationDto
    {
        public TenderInvitationDto()
        {
            InvitationAccessDefinitions = new List<InvitationAccessDefinitionDto>();
        }

        public int Id { get; set; }

        public int TenderId { get; set; }

        public string ContractorId { get; set; } = null!;

        public TenderInvitationStatus Status { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public string? Notes { get; set; }

        public List<InvitationAccessDefinitionDto>? InvitationAccessDefinitions { get; set; }
    }
}
