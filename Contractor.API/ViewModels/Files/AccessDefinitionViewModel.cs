using System.ComponentModel.DataAnnotations;

namespace Contractor.ViewModels.Files
{
    public class ShareAccessDefinitionViewModel
    {
        public int Id { get; set; }

        [Required]
        public List<string> SharedWithIdList { get; set; } = null!;
    }
}
