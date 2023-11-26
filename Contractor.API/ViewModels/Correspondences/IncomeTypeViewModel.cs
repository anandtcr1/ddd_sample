using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Correspondences
{
    public class IncomeTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ArabicDescription { get; set; } = null!;

        [Required]
        public string EnglishDescription { get; set; } = null!;
    }

    public class GetIncomeTypeListViewModel : GetListViewModel
    {
        public string? ArabicDescription { get; set; }

        public string? EnglishDescription { get; set; }
    }
}
