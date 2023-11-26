using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Subscriptions
{
    public class SubscriptionConstants
    {
        public const string DefaultSubscriptionName = "DefaultSubscription";
        public const float DefaultSubscriptionStorageSpace = 1073741824;
        
        public const string OwnerFile = "Owner File";
        public const string Correspondence = "Correspondence";
        public const string Tenders = "Tenders";
        public const string Finance = "Finance";
        public const string Approvals = "Approvals";
        public const string Payments = "Payments";
        public const string SharedProjectFiles = "Shared Project Files";

        public static readonly IList<string> DefaultSubscriptionFoldersList = new ReadOnlyCollection<string>
        (new List<string> { OwnerFile, Tenders, Finance, Approvals, Payments, SharedProjectFiles, Correspondence });
    }
}
