using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Exceptions
{
    public static class CustomValidationExceptionConstants
    {
        public const string ValidationException = "ValidationException";
    }

    public static class CustomValidationExceptionErrorMessages
    {
        public const string InvalidNumber = "invalid number";
        public const string UserProfileExist = "UserProfileExist";
        public const string InvalidString = "invalid string";
        public const string TakenPhoneNumber = "Phone Number '{0}' is already taken.";
    }
}
