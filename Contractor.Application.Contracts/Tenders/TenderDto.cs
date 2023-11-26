
namespace Contractor.Tenders
{
    public class TenderDto
    {
        public TenderDto()
        {
            TenderAccessDefinitions = new List<TenderAccessDefinitionDto>();
            TenderInvitations = new List<TenderInvitationDto>();
        }
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public DateTime IssueDate { get; set; }

        public string Title { get; set; } = null!;

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        public TenderStatus Status { get; set; }

        public int ProjectId { get; set; }

        public string? Note { get; set; }

        public List<TenderAccessDefinitionDto>? TenderAccessDefinitions { get; set; }

        public List<TenderInvitationDto>? TenderInvitations { get; set; }
    }
}
