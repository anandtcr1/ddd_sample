using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public static class CustomClaimTypes
    {
        public static readonly string Page = "Page";
        public static readonly string UserType = "UserType";
        public static readonly string Functionality = "Functionality";
        public static readonly string Impersonation = "Impersonation";
        public static readonly string ImpersonatorId = "ImpersonatorId";
    }
    public static class PageNames
    {
        public const string RoleManagment = "RoleManagment";
        public const string PageManagment = "PageManagment";
        public const string UserManagment = "UserManagment";
        public const string DefinitionFiles = "DefinitionFiles";
        public const string DraftList = "DraftList";
        public const string SendInvitationForDraft = "SendInvitationForDraft";
        public const string SubscriptionManagment = "SubscriptionManagment";
        public const string ProjectManagement = "ProjectManagement";

        public const string CorrespondenceList = "Correspondence List";
        public const string TenderList = "Tender List"; 
        public const string Reports = "Reports";
        public const string FilesManagement = "Files Management";
        public const string ConsultantList = "Consultant List";

        public static List<string> AllPages = new List<string>
        {
            RoleManagment,
            PageManagment,
            UserManagment,
            DefinitionFiles,
            DraftList,
            SendInvitationForDraft,
            SubscriptionManagment,
            ProjectManagement,
            CorrespondenceList,
            Reports,
            FilesManagement,
            ConsultantList,
            TenderList
        };

        public static List<string> ConsultantPages = new List<string>
        {
            CorrespondenceList, 
            ProjectManagement,
            FilesManagement,
            Reports,
            SubscriptionManagment,
            TenderList
        };

        public static List<string> ContractorPages = new List<string>
        {
            CorrespondenceList, 
            ProjectManagement,
            FilesManagement,
            Reports,
            SubscriptionManagment,
            TenderList
        };

        public static List<string> OwnerPages = new List<string>
        {
            CorrespondenceList,
            ProjectManagement,
            FilesManagement,
            Reports,
            SubscriptionManagment,
            ConsultantList,
            SendInvitationForDraft,
        };
    }

    public static class FunctionalityNames
    {
        public const string Impersonation = "Impersonation";
        public const string CreatePorject = "CreatePorject";
        public const string InviteProjectOwner = "InviteProjectOwner";
        public const string ViewProjectTender = "ViewProjectTender";

        public static List<string> AllFunctionalities = new List<string>
        {
            Impersonation,
            CreatePorject,
            InviteProjectOwner,
            ViewProjectTender
        };

        public static List<string> ConsultantFunctionalities = new List<string>
        {
            CreatePorject,
            InviteProjectOwner,
            ViewProjectTender
        };

        public static List<string> ContractorFunctionalities = new List<string>
        {
        };

        public static List<string> OwnerFunctionalities = new List<string>
        {
        };
    }
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Consultant = "Consultant";
        public const string Contractor = "Contractor";
        public const string Owner = "Owner";

        public static readonly List<string> Roles = new()
        {
            Admin,
            Consultant,
            Contractor,
            Owner
        };
    }

    public static class SeedUserNames
    {
        public const string Admin = "admin@salina.group";
        public const string Consultant = "consultant@salina.group";
        public const string Contractor = "contractor@salina.group";
        public const string Owner = "owner@salina.group";

        public static List<(string, UserTypes)> Users = new List<(string, UserTypes)> {
            (Admin, UserTypes.Admin),
            (Consultant, UserTypes.Consultant),
            (Owner, UserTypes.Owner),
            (Contractor, UserTypes.Contractor)
        };

    }

    public enum UserTypes
    {
        Consultant,
        Contractor,
        Admin,
        Owner,
        SubOwner,
        SubConsultant,
        SubContractor
    }

    public enum UserStatus
    {
        Active,
        Suspended,
        Removed
    }

    public enum ProfileAccessDefinitionType
    {
        Picture,
        Cover,
        Attachment
    }
}
