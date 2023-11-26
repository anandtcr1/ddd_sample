using Contractor.Correspondences;
using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Tenders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class AccessDefinition
    {
        private AccessDefinition()
        {

        }
        private AccessDefinition(string path, 
            string userId, 
            bool isOriginal, 
            AccessDefinitionStatus status, 
            bool deletable, 
            AccessDefinitionType type, 
            int? parentId,
            File? file,
            int? originalId)
        {
            Path = path;
            UserId = userId;
            Status = status;
            IsOriginal = isOriginal;
            Deletable = deletable;
            Type = type;
            ParentId = parentId;
            OriginalId = originalId;
            File = file;
            Copies = new HashSet<AccessDefinition>();
            Children = new HashSet<AccessDefinition>();
            TenderAccessDefinitions = new HashSet<TenderAccessDefinition>();
            CorrespondenceAccessDefinitions = new HashSet<CorrespondenceAccessDefinition>();
            InvitationAccessDefinitions = new HashSet<InvitationAccessDefinition>();
            ProfileAccessDefinitions = new HashSet<ProfileAccessDefinition>();
        }

        [Key]
        public int Id { get; private set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; private set; }

        [ForeignKey("Original")]
        public int? OriginalId { get; private set; }

        public int? FileId { get; private set; }

        [Required]
        public string Path { get; private set; } = null!;

        public bool IsOriginal { get; private set; }

        [Required]
        public string UserId { get; private set; } = null!;

        public AccessDefinitionStatus Status { get; private set; }
        
        public AccessDefinitionType Type { get; private set; }

        public bool Deletable { get; private set; }

        public virtual User? User { get; private set; }


        public virtual File? File { get; private set; }

        public virtual AccessDefinition? Parent { get; private set; }

        public virtual AccessDefinition? Original { get; private set; }

        public virtual ICollection<AccessDefinition>? Copies { get; private set; }

        public virtual ICollection<AccessDefinition>? Children { get; private set; }

        public virtual ICollection<TenderAccessDefinition> TenderAccessDefinitions { get; private set; }

        public virtual ICollection<InvitationAccessDefinition> InvitationAccessDefinitions { get; private set; }

        public virtual ICollection<CorrespondenceAccessDefinition> CorrespondenceAccessDefinitions { get; private set; }
        
        public virtual ICollection<ProfileAccessDefinition> ProfileAccessDefinitions { get; private set; }



        public static AccessDefinition Create(string path, 
            string userId, 
            bool isOriginal, 
            AccessDefinitionStatus status, 
            bool deletable, 
            AccessDefinitionType type, 
            int? parentId,
            File? file,
            int? originalId)
        {
            if(type == AccessDefinitionType.File && file == null)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            return new AccessDefinition(path, userId, isOriginal, status, deletable, type, parentId, file, originalId);
        }

        public List<AccessDefinition> Share(List<string> sharedWithIdList)
        {
            List<AccessDefinition> list = new();

            if (!IsOriginal)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            foreach (string sharedWithId in sharedWithIdList)
            {
                list.Add(new AccessDefinition(Path, sharedWithId, false, AccessDefinitionStatus.Pending, true, Type, null, File, Id));
            }

            return list;
        }

        public void AddTenderAccessDefinition(TenderAccessDefinition  tenderAccessDefinition)
        {
            TenderAccessDefinitions.Add(tenderAccessDefinition);
        }

        public void AddProfileAccessDefinition(ProfileAccessDefinition profileAccessDefinition)
        {
            ProfileAccessDefinitions.Add(profileAccessDefinition);
        }

        public void AddCorrespondenceAccessDefinition(CorrespondenceAccessDefinition correspondenceAccessDefinition)
        {
            CorrespondenceAccessDefinitions.Add(correspondenceAccessDefinition);
        }

        public void AddInvitationAccessDefinition(InvitationAccessDefinition invitationAccessDefinition)
        {
            InvitationAccessDefinitions.Add(invitationAccessDefinition);
        }

        public void AddChild(AccessDefinition accessDefinition)
        {
            Children.Add(accessDefinition);
        }
    }
}
