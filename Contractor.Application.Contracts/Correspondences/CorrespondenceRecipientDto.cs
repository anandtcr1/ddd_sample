
namespace Contractor.Correspondences
{
    public class CorrespondenceRecipientDto
    {
        public int CorrespondenceId { get; set; }

        public string RecipientId { get; set; } = null!;

        public CorrespondenceRecipientType RecipientType { get; set; }

        //TODO to be removed in the next visit
        public string? RecipientName { get; set; }
    }
}
