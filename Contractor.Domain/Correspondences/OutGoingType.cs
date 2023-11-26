using System.ComponentModel.DataAnnotations;

namespace Contractor.Correspondences
{
    public class OutGoingType
    {
        private OutGoingType(string arabicDescription, string englishDescription)
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


        public static OutGoingType Create(string arabicDescription, string englishDescription)
        {
            return new OutGoingType(arabicDescription, englishDescription);
        }

        public void Update(string arabicDescription, string englishDescription)
        {
            ArabicDescription = arabicDescription;
            EnglishDescription = englishDescription;
        }
    }
}
