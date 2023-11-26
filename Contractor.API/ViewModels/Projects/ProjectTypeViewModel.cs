using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Projects
{
    public class ProjectTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ArabicDescription { get; set; } = null!;

        [Required]
        public string EnglishDescription { get; set; } = null!;
    }

    public class GetProjectTypeListViewModel : GetListViewModel
    {
        public string? ArabicDescription { get; set; }

        public string? EnglishDescription { get; set; }
    }
}
