using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public class Area
    {
        private Area(string arabicName, string englishName, int cityId)
        {
            CityId = cityId;
            ArabicName = arabicName;
            EnglishName = englishName;
        }


        [Key]
        public int Id { get; private set; }

        [Required]
        public string ArabicName { get; private set; } = null!;

        [Required]
        public string EnglishName { get; private set; } = null!;

        public int CityId { get; private set; }


        public virtual City? City { get; private set; }


        public static Area Create(string arabicName, string englishName, int cityId)
        {
            return new Area(arabicName, englishName, cityId);
        }

        public void Update(string arabicName, string englishName, int cityId)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
            CityId = cityId;
        }
    }
}
