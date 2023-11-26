using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public class Nationality
    {
        private Nationality(string arabicName, string englishName)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string ArabicName { get; private set; } = null!;

        [Required]
        public string EnglishName { get; private set; } = null!;


        public static Nationality Create(string  arabicName, string englishName)
        {
            return new Nationality(arabicName, englishName);
        }

        public void Update(string arabicName,string englishName)
        {
            ArabicName  = arabicName;
            EnglishName = englishName;
        }
    }
}
