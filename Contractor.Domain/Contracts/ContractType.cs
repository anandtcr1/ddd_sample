using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Contracts
{
    public class ContractType
    {
        private ContractType(string arabicDescription, string englishDescription)
        {
            ArabicDescription = arabicDescription;
            EnglishDescription = englishDescription;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string ArabicDescription { get; private set; }

        [Required]
        public string EnglishDescription { get; private set; }

        public static ContractType Create(string arabicDescription, string englishDescription)
        {
            return new ContractType(arabicDescription, englishDescription);
        }

        public void Update(string arabicDescription, string englishDescription)
        {
            ArabicDescription = arabicDescription;
            EnglishDescription = englishDescription;
        }
    }
}
