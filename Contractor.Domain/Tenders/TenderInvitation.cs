using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class TenderInvitation
    {

        private TenderInvitation(int tenderId, string contractorId )
        {
            TenderId = tenderId;
            ContractorId = contractorId;
            Status = TenderInvitationStatus.Pending;
        }

        [Key]
        public int Id { get; private set; }

        public int TenderId { get; private set; }

        [Required]
        public string ContractorId { get; private set; } = null!;

        public TenderInvitationStatus Status { get; private set; }

        public DateTime? SubmissionDate { get; private set; }

        public string? Notes { get; private set; }

        public virtual Tender Tender { get; private set; }

        public virtual User Contractor { get; private set; }

        public virtual ICollection<InvitationAccessDefinition> InvitationAccessDefinitions { get; private set; }
         
        public void Accept()
        {
            if (Status != TenderInvitationStatus.Pending)
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);

            Status = TenderInvitationStatus.Accepted;
        }

        public void Decline()
        {
            if (Status != TenderInvitationStatus.Pending)
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);

            Status = TenderInvitationStatus.Rejected;
        }

        public void Submit(string note)
        {
            if (Status != TenderInvitationStatus.Accepted)
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);

            Status = TenderInvitationStatus.Submitted;
            Notes = note;
            SubmissionDate = DateTime.Now;
        }

        public void SelectWinner()
        {
            if (Status != TenderInvitationStatus.Submitted)
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);

            Status = TenderInvitationStatus.Winner;
        }

        public InvitationAccessDefinition AddInvitationAccessDefinition()
        {
            var invitationAccessDefinition = InvitationAccessDefinition.Create();

            InvitationAccessDefinitions.Add(invitationAccessDefinition);

            return invitationAccessDefinition;
        }
        
        public static TenderInvitation Create(int tenderId, string contractorId)
        {
            return new TenderInvitation(tenderId, contractorId);
        }


    }
}
