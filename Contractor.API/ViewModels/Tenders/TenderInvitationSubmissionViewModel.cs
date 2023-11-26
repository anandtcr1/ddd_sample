using System.ComponentModel.DataAnnotations;

namespace Contractor.Tenders
{
    public class TenderInvitationSubmissionViewModel
    {
        public int TenderId { get; set; }

        [Required]
        public string Note { get; set; } = null!;

        [Required]
        public List<IFormFile> Files { get; set; } = default!;
    }
}
