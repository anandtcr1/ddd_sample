using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Lookups
{
    public class NationalityViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public string EnglishName { get; set; } = null!;
    }


    public class GetNationalityListViewModel : GetListViewModel
    {
        public string? ArabicName { get; set; } = null!;

        public string? EnglishName { get; set; } = null!;
    }
}
