using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Lookups
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public string EnglishName { get; set; } = null!;
    }

    public class AreaViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public string EnglishName { get; set; } = null!;

        public int CityId { get; set; }
    }

    public class GetCityListViewModel : GetListViewModel
    {
        public string? ArabicName { get; set; } = null!;

        public string? EnglishName { get; set; } = null!;
    }

    public class GetAreaListViewModel : GetListViewModel
    {
        public int CityId { get; set; }

        public string? ArabicName { get; set; } = null!;

        public string? EnglishName { get; set; } = null!;
    }
}
