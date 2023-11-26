
namespace Contractor.Correspondences
{
    public class CorrespondenceDto
    {
        public CorrespondenceDto()
        {
            CorrespondenceAccessDefinitions = new List<CorrespondenceAccessDefinitionDto>();
            CorrespondenceRecipients = new List<CorrespondenceRecipientDto>();
        }

        public int Id { get; set; }

        public int ThreadId { get; set; }

        public string Number { get; set; } = null!;

        public DateTime IssueDate { get; set; }

        public string? ReferenceNumber { get; set; }

        public int ProjectId { get; set; }

        public string Subject { get; set; } = null!;

        public string Content { get; set; } = null!;

        public List<CorrespondenceAccessDefinitionDto> CorrespondenceAccessDefinitions { get; set; }

        public List<CorrespondenceRecipientDto> CorrespondenceRecipients { get; set; }
    }
}
