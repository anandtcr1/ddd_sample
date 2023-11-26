using Contractor.Files;
using System.ComponentModel.DataAnnotations;

namespace Contractor.ViewModels.Files
{
    public class AccessDefinitionViewModel
    {
        [Required]
        public IFormFile FormFile { get; set; } = null!;

        public int ParentId { get; set; }
    }
}
