using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Exceptions
{
    [Serializable]
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException() { }

        public BusinessRuleException(string message)
            : base(message) { }

        public BusinessRuleException(string message, Exception inner)
            : base(message, inner) { }
    }
}
