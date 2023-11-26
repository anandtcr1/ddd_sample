using Contractor.Identities;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Correspondences
{
    public class CorrespondenceRecipient
    {
        private CorrespondenceRecipient(string recipientId, CorrespondenceRecipientType recipientType)
        {

            RecipientId = recipientId;
            RecipientType = recipientType;
        }

        public int Id { get; private set; }

        public int CorrespondenceId { get; private set; }

        [Required]
        public string RecipientId { get; private set; } = null!;

        public CorrespondenceRecipientType RecipientType { get; private set; }


        public virtual Correspondence? Correspondence { get; private set; }

        public virtual User Recipient { get; private set; }


        public static CorrespondenceRecipient Create(string recipientId, CorrespondenceRecipientType recipientType)
        {
            return new CorrespondenceRecipient(recipientId, recipientType);
        }
    }
}
