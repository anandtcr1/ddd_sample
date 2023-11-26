using System.ComponentModel.DataAnnotations;

namespace Contractor.Correspondences
{
    public class IncomeType
    {
        private IncomeType(string arabicDescription, string englishDescription)
        {
            ArabicDescription = arabicDescription;
            EnglishDescription = englishDescription;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string ArabicDescription { get; private set; } = null!;

        [Required]
        public string EnglishDescription { get; private set; } = null!;


        public static IncomeType Create(string arabicDescription, string englishDescription)
        {
            return new IncomeType(arabicDescription, englishDescription);
        }

        public void Update(string arabicDescription, string englishDescription)
        {
            ArabicDescription = arabicDescription;
            EnglishDescription = englishDescription;
        }
    }
}
