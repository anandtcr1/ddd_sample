using Contractor.Correspondences;
using Contractor.Exceptions;
using Contractor.Files;
using Contractor.GeneralEntities;
using Contractor.Lookups;
using Contractor.Projects;
using Contractor.Subscriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contractor.Identities
{
    public class User : IdentityUser, IAuditEntity
    {
        private User()
        {
        }
        //TODO remove IAuditEntity Logic to Context
        private User(string displayName, string email, string? phoneNumber, UserTypes userType , Company company, string insertUserId)
        {
            Id = Guid.NewGuid().ToString();
            DisplayName = displayName;
            UserName = email;
            Email = email;
            PhoneNumber = phoneNumber;
            LockoutEnabled = false;
            ConsultantDraftProjects = new HashSet<DraftProject>();
            OwnerDraftProjects = new HashSet<DraftProject>();
            Claims = new HashSet<UserClaim>();
            Roles = new HashSet<UserRole>();
            Projects = new HashSet<ProjectUser>();
            CorrespondenceRecipients = new HashSet<CorrespondenceRecipient>();
            UserType = userType;
            Company = company;
            InsertUserId = insertUserId;
            InsertDate = DateTime.Now;
        }

        [Required]
        public string DisplayName { get; private set; } = null!;
        
        public UserTypes UserType { get; private set; }
        
        public int? SubscriptionPlanId { get; private set; }      
        
        public int CompanyId { get; private set; }

        public DateTime InsertDate { get; set; }

        public string InsertUserId { get; set; }
        
        public DateTime? LastModifiedDate { get; set; }
        
        public string? LastModifyUserId { get; set; }
        
        public DateTime? DeleteDate { get; set; }
        
        public string? DeleteUserId { get; set; }

        public UserStatus Status { get; private set; }

        public virtual ICollection<UserClaim> Claims { get; private set; }

        public virtual ICollection<UserRole> Roles { get; private set; }

        public virtual ICollection<DraftProject> OwnerDraftProjects { get; private set; }

        public virtual ICollection<DraftProject> ConsultantDraftProjects { get; private set; }
        
        public virtual ICollection<CorrespondenceRecipient> CorrespondenceRecipients { get; private set; }

        public virtual UserProfile? Profile { get; private set; }

        public virtual ICollection<ProjectUser> Projects { get; private set; }

        public virtual Subscription? SubscriptionPlan { get; private set; } 
        
        public virtual Company? Company { get; private set; }

        

        public static User Create(string displayName, string email, string? phoneNumber, UserTypes userType , Company company, string insertUserId)
        {
            return new User(displayName, email, phoneNumber, userType , company, insertUserId);
        }

        public void Update(string displayName, string lastModifyUserId)
        {
            DisplayName = displayName;
            LastModifyUserId = lastModifyUserId;
            LastModifiedDate = DateTime.Now;
        }

        public void CreateProfile(
            string firstName,
            string lastName,
            DateTime birthDate,
            string? address,
            Gender gender,
            string? alternativeEmail,
            int areaId,
            int nationalityId)
        {
            if (Profile != null)
            {
                throw new CustomValidationException(nameof(UserProfile), CustomValidationExceptionErrorMessages.UserProfileExist);
            }

            var profile = UserProfile.Create(Id, firstName, lastName, birthDate, address, gender, alternativeEmail, areaId, nationalityId);

            Profile = profile;
        }


        public void UpdateProfile(
            string firstName,
            string lastName,
            DateTime birthDate,
            string? address,
            Gender gender,
            string? alternativeEmail,
            int areaId,
            int nationalityId)
        {
            if (Profile == null)
            {
                throw new EntityNotFoundException(AuthenticationExceptionConstants.UserNotFound, nameof(UserProfile), Id);
            }

            Profile.Update(firstName, lastName, birthDate, address, gender, alternativeEmail, areaId, nationalityId);
        }

        public (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewPictureAccessDefinition) ChangeProfilePicture()
        {
            return Profile.ChangeProfilePicture();
        }

        public (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewCoverAccessDefinition) ChangeProfileCover()
        {
            return Profile.ChangeProfileCover();
        }

        public void UpdateSubscriptionPlanId(int? subscriptionPlanId)
        {
            this.SubscriptionPlanId = subscriptionPlanId;
        }

        public void AddPageClaims(List<Tuple<int, string>> pages)
        {
            var pagesClaims = pages.Select(x => UserClaim.Create(Id, x.Item1, CustomClaimTypes.Page, x.Item2)).ToList();

            foreach (var page in pagesClaims)
            {
                Claims.Add(page);
            }
        }

        public void UpdatePageClaims(List<Tuple<int, string>> pages)
        {
            Claims = Claims.Where(x => x.ClaimType != CustomClaimTypes.Page).ToList();

            AddPageClaims(pages);
        }

        public void AddFunctionalityClaims(List<string> Functionalities)
        {
            var claims = Functionalities.Select(x => UserClaim.Create(Id, null, CustomClaimTypes.Functionality, x)).ToList();

            foreach (var item in claims)
            {
                Claims.Add(item);
            }
        }

        public void UpdateFunctionalityClaims(List<string> Functionalities)
        {
            Claims = Claims.Where(x => x.ClaimType != CustomClaimTypes.Functionality).ToList();

            AddFunctionalityClaims(Functionalities);
        }

        public void SuspendUser(string lastModifyUserId)
        {
            if(Status != UserStatus.Active)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidStatusChange);
            }

            Status = UserStatus.Suspended;
            LastModifyUserId = lastModifyUserId;
            LastModifiedDate = DateTime.Now;
        }

        public void RemoveUser(string deleteUserId)
        {
            if (Status == UserStatus.Removed)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidStatusChange);
            }

            Status = UserStatus.Removed;
            DeleteUserId = deleteUserId;
            DeleteDate = DateTime.Now;
        }

        public void ActivateUser(string lastModifiedUserId)
        {
            if (Status == UserStatus.Active)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.InvalidStatusChange);
            }

            Status = UserStatus.Active;
            LastModifyUserId = lastModifiedUserId;
            LastModifiedDate = DateTime.Now;
        }

    }
}
