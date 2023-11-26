using Contractor.Correspondences;
using Contractor.Exceptions;
using Contractor.Identities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class Project
    {
        private Project(int projectTypdId, string projectNumber, string projectTitle, DateTime startDate, DateTime endDate, string description)
        {
            ProjectTypdId = projectTypdId;
            ProjectNumber = projectNumber;
            ProjectTitle = projectTitle;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            ProjectUsers = new HashSet<ProjectUser>();
            ProjectInvitations = new HashSet<ProjectInvitation>();
            Status = ProjectStatus.Active;
            Correspondences = new HashSet<Correspondence>();
        }

        [Key]
        public int Id { get; private set; }

        [ForeignKey("DraftProject")]
        public int? DraftProjectId { get; private set; }

        [ForeignKey("ProjectType")]
        public int ProjectTypdId { get; private set; }

        public string ProjectNumber { get; private set; } = null!;

        public string ProjectTitle { get; private set; } = null!;

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public string Description { get; private set; } = null!;

        public ProjectStatus Status { get; private set; }


        public virtual DraftProject? DraftProject { get; private set; }

        public virtual ProjectType? ProjectType { get; private set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; private set; }
        
        public virtual ICollection<ProjectInvitation> ProjectInvitations { get; private set; }
        
        public virtual ICollection<Correspondence> Correspondences { get; private set; }



        public static Project Create(int projectTypdId, string projectNumber, string projectTitle, DateTime startDate, DateTime endDate, string description)
        {
            return new Project(projectTypdId, projectNumber, projectTitle, startDate, endDate, description);
        }

        public void Update(int projectTypdId, string projectTitle, DateTime startDate, DateTime endDate, string description, ProjectStatus status)
        {
            ProjectTypdId = projectTypdId;
            ProjectTitle = projectTitle;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Status = status;
        }

        public void SetDraftProjectId(int draftProjectId)
        {
            DraftProjectId = draftProjectId;
        }

        public void AddOwner(string ownerId)
        {
            if(ProjectUsers.Any(x=>x.UserType == ProjectUserType.Owner))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.MoreThanOneProjectOwner);
            }

            ProjectUsers.Add(ProjectUser.Create(canSeeTenders: false, canAddUsers: true, canEditProject: false, userType: ProjectUserType.Owner, Id, userId: ownerId));
        }

        public void AddConsultant(string consultantId)
        {
            if (ProjectUsers.Any(x => x.UserId == consultantId))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.UserAlreadyAddedToProject);
            }

            ProjectUsers.Add(ProjectUser.Create(canSeeTenders: true, canAddUsers: true, canEditProject: true, userType: ProjectUserType.Consultant, Id, userId: consultantId));
        }

        public void AddSubConsultant(string userId, List<string> subConsultantIdList, List<string> companySubUsers)
        {
            var projectUser = ProjectUsers.FirstOrDefault(x=>x.UserId == userId);

            if(projectUser == null || projectUser.UserType != ProjectUserType.Consultant)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            foreach (var subUserId in companySubUsers)
            {
                var subUser = ProjectUsers.FirstOrDefault(x => x.UserId == subUserId);

                if (subUser != null)
                {
                    ProjectUsers.Remove(subUser);
                }
            }

            foreach (var subConsultantId in subConsultantIdList)
            {
                if (ProjectUsers.Any(x => x.UserId == subConsultantId))
                {
                    continue;
                }

                ProjectUsers.Add(ProjectUser.Create(canSeeTenders: false, canAddUsers: false, canEditProject: false, userType: ProjectUserType.SubConsultant, Id, userId: subConsultantId));
            }
        }

        public void AddContractor(string userId, List<string> contractorIdList, List<string> companySubUsers)
        {
            var projectUser = ProjectUsers.FirstOrDefault(x => x.UserId == userId);

            if (projectUser == null || (projectUser.UserType != ProjectUserType.Consultant && projectUser.UserType != ProjectUserType.SubConsultant))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            foreach (var subUserId in companySubUsers)
            {
                var subUser = ProjectUsers.FirstOrDefault(x => x.UserId == subUserId);

                if (subUser != null)
                {
                    ProjectUsers.Remove(subUser);
                }
            }

            foreach (var contractorId in contractorIdList)
            {
                if (ProjectUsers.Any(x => x.UserId == contractorId))
                {
                    continue;
                }

                ProjectUsers.Add(ProjectUser.Create(canSeeTenders: false, canAddUsers: true, canEditProject: false, userType: ProjectUserType.Contractor, Id, userId: contractorId));
            }
        }

        public void AddSubContractor(string userId, List<string> subContractorIdList, List<string> companySubUsers)
        {
            var projectUser = ProjectUsers.FirstOrDefault(x => x.UserId == userId);

            if (projectUser == null || projectUser.UserType != ProjectUserType.Contractor)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            foreach (var subUserId in companySubUsers)
            {
                var subUser = ProjectUsers.FirstOrDefault(x => x.UserId == subUserId);

                if (subUser != null)
                {
                    ProjectUsers.Remove(subUser);
                }
            }

            foreach (var subContractorId in subContractorIdList)
            {
                if (ProjectUsers.Any(x => x.UserId == subContractorId))
                {
                    continue;
                }

                ProjectUsers.Add(ProjectUser.Create(canSeeTenders: false, canAddUsers: false, canEditProject: false, userType: ProjectUserType.SubContractor, Id, userId: subContractorId));
            }
        }

        public void AddSubUser(string userId, List<string> addSubUserIdList, List<string> companySubUsers)
        {
            var projectUser = ProjectUsers.FirstOrDefault(x => x.UserId == userId);

            if (projectUser == null)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            foreach (var subUserId in companySubUsers)
            {
                var subUser = ProjectUsers.FirstOrDefault(x=>x.UserId == subUserId);

                if(subUser != null)
                {
                    ProjectUsers.Remove(subUser);
                }
            }

            foreach (var subUserId in addSubUserIdList)
            {
                if (ProjectUsers.Any(x => x.UserId == subUserId))
                {
                    continue;
                }

                ProjectUserType? userType = null;

                switch (projectUser.UserType)
                {
                    case ProjectUserType.Contractor:
                        userType = ProjectUserType.SubContractor;
                        break;
                    case ProjectUserType.Consultant:
                        userType = ProjectUserType.SubConsultant;
                        break;
                    case ProjectUserType.Owner:
                        userType = ProjectUserType.SubOwner;
                        break;
                    default: throw new BusinessRuleException(BusinessRuleExceptionConstants.OperationNotValid);
                }


                ProjectUsers.Add(ProjectUser.Create(canSeeTenders: false, canAddUsers: false, canEditProject: false, userType: userType!.Value, Id, userId: subUserId));
            }
        }

        public void AddInvitation(string userId, string email, ProjectUserType userType)
        {
            var projectUser = ProjectUsers.FirstOrDefault(x => x.UserId == userId);

            if (projectUser == null)
            { 
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            var oldinvite = ProjectInvitations.FirstOrDefault(x => x.Email == email && x.Status == ProjectInvitationStatus.Pending);
            if(oldinvite != null)
            {
                ProjectInvitations.Remove(oldinvite);
            }

            ProjectInvitations.Add(ProjectInvitation.Create(email, Id, ProjectInvitationStatus.Pending, userType));
        }

        public void AcceptInvitation(string userId, string email)
        {
            var projectInvitation = ProjectInvitations.FirstOrDefault(x => x.Email == email) ?? throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound);

            projectInvitation.Accept();

            AddOwner(userId);

        }

        public void DeclineInvitation(string userId, string email)
        {
            var projectInvitation = ProjectInvitations.FirstOrDefault(x => x.Email == email) ?? throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound);

            projectInvitation.Decline();

        }

    }
}
