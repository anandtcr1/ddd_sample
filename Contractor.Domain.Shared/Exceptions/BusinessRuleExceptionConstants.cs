using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Exceptions
{
    public static class BusinessRuleExceptionConstants
    {
        public const string OperationNotValid = "OperationNotValid";
        public const string UserLockout = "UserLockout";
        public const string StatusIdNotChanged = "StatusIdNotChanged";
        public const string InvalidStatusChange = "InvalidStatusChange";
        public const string InvalidUserType = "InvalidUserType";
        public const string PathNotFound = "PathNotFound";
        public const string AccessDenied = "AccessDenied";
        public const string DuplicateProjectNumber = "DuplicateProjectNumber";
        public const string MoreThanOneProjectOwner = "MoreThanOneProjectOwner";
        public const string UserAlreadyAddedToProject = "UserAlreadyAddedToProject";
    }
}
