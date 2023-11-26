using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Exceptions
{
    [Serializable]
    public class CustomValidationException : Exception
    {
        public string Property { get; }
        public string Error { get; }

        /// <summary>
        /// Exception for Property validation, Property {property}: {error}. 
        /// </summary>
        public CustomValidationException(string property, string error)
            : base($"Property {property}: {error}")
        {
            Property = property;
            Error = error;
        }

        /// <summary>
        /// Exception for Property validation
        /// </summary>
        /// <param name="message">localized message with two parameter {property:0} {error:1}</param>
        /// <param name="property"></param>
        /// <param name="error"></param>
        public CustomValidationException(string message, string property, string error)
            : base(string.Format(message, property, error))
        {
            Property = property;
            Error = error;
        }

    }
}
