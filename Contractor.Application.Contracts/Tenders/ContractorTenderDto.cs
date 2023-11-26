using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class ContractorTenderDto
    {
        public ContractorTenderDto()
        {
            TenderAccessDefinitions = new List<TenderAccessDefinitionDto>();
        }
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public DateTime IssueDate { get; set; }

        public string Title { get; set; } = null!;

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        public TenderStatus Status { get; set; }

        public int ProjectId { get; set; }

        public string ProjectNumber { get; set; }

        public string ProjectTitle { get; set; }

        public string? Note { get; set; }

        public List<TenderAccessDefinitionDto>? TenderAccessDefinitions { get; set; }

        public TenderInvitationDto TenderInvitation { get; set; }
    }
}
