using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Correspondences
{
    public class CorrespondenceViewModel
    {
        public string? ReferenceNumber { get; set; }

        public int ProjectId { get; set; }

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public List<IFormFile>? CorrespondenceFiles { get; set; }

        [Required]
        public List<string> ToRecipients { get; set; } = default!;

        public List<string>? CCRecipients { get; set; }
    }

    public class CorrespondenceReplayViewModel
    {
        public int OriginalId { get; set; }

        public string? ReferenceNumber { get; set; }

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public List<IFormFile>? CorrespondenceFiles { get; set; }

    }

    public class GetCorrespondenceListViewModel : GetListViewModel
    {
        public string? Search { get; set; }

    }
}
