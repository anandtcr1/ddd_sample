using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Correspondences
{
    public class OutGoingTypeDto
    {
        public int Id { get; private set; }

        public string ArabicDescription { get; set; } = null!;

        public string EnglishDescription { get; set; } = null!;
    }
}
