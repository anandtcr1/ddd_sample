using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Projects;
using Contractor.Tools;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Tenders
{
    public class Tender
    {
        private Tender(string number, 
            DateTime issueDate, 
            string title, 
            DateTime openDate, 
            DateTime closeDate, 
            TenderStatus status, 
            int projectId, 
            string? note)
        {
            Number = number;
            IssueDate = issueDate;
            Title = title;
            OpenDate = openDate;
            CloseDate = closeDate;
            Status = status;
            ProjectId = projectId;
            Note = note;
            TenderAccessDefinitions = new HashSet<TenderAccessDefinition>();
            TenderInvitations = new HashSet<TenderInvitation>();
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Number { get; private set; } = null!;

        public DateTime IssueDate { get; private set; }

        public string Title { get; private set; } = null!;

        public DateTime OpenDate { get; private set; }
        
        public DateTime CloseDate { get; private set; }

        public TenderStatus Status { get; private set; }

        public int ProjectId { get; private set; }

        public string? Note { get; private set; }


        public virtual Project Project { get; private set; }

        public virtual ICollection<TenderAccessDefinition> TenderAccessDefinitions { get; private set; }
        
        public virtual ICollection<TenderInvitation> TenderInvitations { get; private set; }

        public static Tender Create(int maxId,
            string title,
            DateTime openDate,
            DateTime closeDate,
            TenderStatus status,
            int projectId,
            string? note)
        {            
            string number = GeneralTools.GenerateNumber(maxId);

            var issueDate = DateTime.Now;
            return new Tender(number, issueDate, title, openDate, closeDate, status, projectId, note);
        }

        public void Update(string title,
            DateTime openDate,
            DateTime closeDate,
            TenderStatus status,
            string? note)
        {
            if(status == TenderStatus.Draft && status != Status)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            Title = title;
            OpenDate = openDate;
            CloseDate = closeDate;
            Status = status;
            Note = note;
        }

        public TenderAccessDefinition AddTenderAccessDefinition()
        {
            TenderAccessDefinition tenderAccessDefinition = TenderAccessDefinition.Create();

            TenderAccessDefinitions.Add(tenderAccessDefinition);

            return tenderAccessDefinition;
        }

        public void RemoveTenderAccessDefinition(int accessDefinitionId)
        {
            TenderAccessDefinition? tenderAccessDefinition = TenderAccessDefinitions.FirstOrDefault(x => x.AccessDefinitionId == accessDefinitionId);

            if(tenderAccessDefinition != null)
            {
                TenderAccessDefinitions.Remove(tenderAccessDefinition);
            }
        }

        public void AcceptInvitation(string userId)
        {
            var invitation = TenderInvitations.FirstOrDefault(x => x.ContractorId == userId) ?? throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);

            invitation.Accept();

        }

        public void DeclineInvitation(string userId)
        {
            var invitation = TenderInvitations.FirstOrDefault(x => x.ContractorId == userId) ?? throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);

            invitation.Decline();

        }

        public TenderInvitation SubmitInvitation(string note, string userId)
        {
            var invitation = TenderInvitations.FirstOrDefault(x => x.ContractorId == userId) ?? throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);

            if(DateTime.Now > CloseDate)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            invitation.Submit(note);

            return invitation;
        }

        public void AddTenderInvitation(List<string> userIdList)
        {
            foreach (string userId in userIdList)
            {
                TenderInvitations.Add(TenderInvitation.Create(Id, userId));
            }
        }

        public TenderInvitation SelectWinner(int invitationId)
        {
            if (Status != TenderStatus.Active)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            var invitation = TenderInvitations.FirstOrDefault(x=>x.Id == invitationId) ?? throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            
            invitation.SelectWinner();

            Status = TenderStatus.Completed;

            return invitation;

        }

        
    }
}
