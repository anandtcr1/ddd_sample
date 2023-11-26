using System.ComponentModel.DataAnnotations;

namespace Contractor.Lookups
{
    public class City
    {
        private City(string arabicName, string englishName)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
            Areas = new HashSet<Area>();
        }


        [Key]
        public int Id { get; private set; }

        [Required]
        public string ArabicName { get; private set; } = null!;

        [Required]
        public string EnglishName { get; private set; } = null!;

        public ICollection<Area> Areas { get; private set; }


        public static City Create(string arabicName, string englishName)
        {
            return new City(arabicName, englishName);
        }

        public void Update(string arabicName, string englishName)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
        }
    }
}
