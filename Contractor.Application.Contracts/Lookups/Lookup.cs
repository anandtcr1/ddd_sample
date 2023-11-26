using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public class Lookup<T>
    {
        public required T Value { get; set; }

        public string EnglishDisplay { get; set; } = null!;

        public string ArabicDisplay { get; set; } = null!;
    }
}
