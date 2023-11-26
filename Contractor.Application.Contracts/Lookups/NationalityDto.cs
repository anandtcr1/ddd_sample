using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public class NationalityDto
    {
        public int Id { get; set; }

        public string ArabicName { get; set; } = null!;

        public string EnglishName { get; set; } = null!;
    }
}
