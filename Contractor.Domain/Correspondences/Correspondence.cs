using Contractor.Exceptions;
using Contractor.Projects;
using Contractor.Tools;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Correspondences
{
    public class Correspondence
    {
        private Correspondence() { }

        private Correspondence(string number,
            DateTime issueDate,
            string? referenceNumber,
            int projectId,
            string subject,
            string content,
            int? threadId = null)
        {
            Number = number;
            IssueDate = issueDate;
            ReferenceNumber = referenceNumber;
            ProjectId = projectId;
            Subject = subject;
            Content = content;
            
            if (threadId.HasValue)
            {
                ThreadId = threadId.Value;
            }
            CorrespondenceAccessDefinitions = new HashSet<CorrespondenceAccessDefinition>();
            CorrespondenceRecipients = new HashSet<CorrespondenceRecipient>();
        }

        [Key]
        public int Id { get; private set; }

        public int ThreadId { get; private set; }

        [Required]
        public string Number { get; private set; } = null!;

        public DateTime IssueDate { get; private set; }

        public string? ReferenceNumber { get; private set; }

        public int ProjectId { get; private set; }        
        
        [Required]
        public string Subject { get; private set; } = null!;
        
        [Required]
        public string Content { get; private set; } = null!;

        public virtual ICollection<CorrespondenceAccessDefinition> CorrespondenceAccessDefinitions { get; private set; }
        
        public virtual ICollection<CorrespondenceRecipient> CorrespondenceRecipients { get; private set; }

        public virtual Project Project { get; private set; }

        public virtual CorrespondenceThread Thread { get; private set; }


        public static Correspondence CreateNew(int maxId,
            string? referenceNumber,
            int projectId,
            string subject,
            string content)
        {
            string number = GeneralTools.GenerateNumber(maxId);

            var issueDate = DateTime.Now;
            return new Correspondence(number,
                issueDate, 
                referenceNumber, 
                projectId,
                subject, 
                content);
        }

        public static Correspondence CreateReplay(int maxId,
            string? referenceNumber,
            int projectId,
            string subject,
            string content,
            int threadId)
        {
            string number = GeneralTools.GenerateNumber(maxId);

            var issueDate = DateTime.Now;
            return new Correspondence(number,
                issueDate,
                referenceNumber,
                projectId,
                subject,
                content,
                threadId);
        }

        public CorrespondenceAccessDefinition AddCorrespondenceAccessDefinitions()
        {
            var correspondenceAccessDefinition = CorrespondenceAccessDefinition.Create();

            CorrespondenceAccessDefinitions.Add(correspondenceAccessDefinition);

            return correspondenceAccessDefinition;
        }

        public void AddSender(string userId)
        {
            if(CorrespondenceRecipients.Any(x=>x.RecipientType == CorrespondenceRecipientType.Sender))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
            }

            var correspondenceRecipient = CorrespondenceRecipient.Create(userId, CorrespondenceRecipientType.Sender);

            CorrespondenceRecipients.Add(correspondenceRecipient);
        }

        public void AddToRecipient(string userId)
        {
            if (CorrespondenceRecipients.Any(x => x.RecipientId == userId))
            {
                return;
            }

            var correspondenceRecipient = CorrespondenceRecipient.Create(userId, CorrespondenceRecipientType.ToReceiver);

            CorrespondenceRecipients.Add(correspondenceRecipient);

        }

        public void AddCCRecipient(string userId)
        {
            if (CorrespondenceRecipients.Any(x => x.RecipientId == userId))
            {
                return;
            }

            var correspondenceRecipient = CorrespondenceRecipient.Create(userId, CorrespondenceRecipientType.CCReceiver);

            CorrespondenceRecipients.Add(correspondenceRecipient);

        }

        public IEnumerable<CorrespondenceRecipient> GetCCRecipient()
        {
            return CorrespondenceRecipients.Where(x=>x.RecipientType == CorrespondenceRecipientType.CCReceiver);
        }

        public IEnumerable<CorrespondenceRecipient> GetToRecipient()
        {
            return CorrespondenceRecipients.Where(x => x.RecipientType == CorrespondenceRecipientType.ToReceiver);
        }

        public CorrespondenceRecipient GetSender()
        {
            return CorrespondenceRecipients.FirstOrDefault(x => x.RecipientType == CorrespondenceRecipientType.Sender)!;
        }
    }
}
