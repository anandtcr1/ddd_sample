using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tools
{
    public static class GeneralTools
    {
        public static string GenerateNumber(int serialno)
        {
            string[] words = { DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), serialno.ToString("D6") };
            return string.Concat(words);
        }
    }
}
