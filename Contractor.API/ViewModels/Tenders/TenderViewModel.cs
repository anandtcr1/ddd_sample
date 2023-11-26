using Contractor.Tenders;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Tenders
{
    public class TenderViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        public TenderStatus Status { get; set; }

        public int ProjectId { get; set; }

        public string? Note { get; set; }

        public List<IFormFile>? Files { get; set; }
    }
}
