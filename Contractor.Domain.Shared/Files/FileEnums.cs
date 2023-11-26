using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public enum FileType
    {
        Other = 0,
        ProfileMedia = 1,
        ProfileAttachment = 2,
        ProjectFiles = 3,
        TenderFiles = 4,
        TenderSubmissionFiles = 5,
        ProfileChat = 6
    }

    public enum AccessDefinitionStatus
    {
        Pending,
        Active
    }

    public enum AccessDefinitionType
    {
        Folder,
        File
    }
}
