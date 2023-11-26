using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contractor.Lookups;
using Contractor.Files;

namespace Contractor.Identities
{
    public class UserProfile
    {
        private UserProfile(string userId,
            string firstName,
            string lastName,
            DateTime birthDate,
            string? address,
            Gender gender,
            string? alternativeEmail,
            int areaId,
            int nationalityId)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Address = address;
            Gender = gender;
            AlternativeEmail = alternativeEmail;
            AreaId = areaId;
            NationalityId = nationalityId;
            ProfileAccessDefinitions = new HashSet<ProfileAccessDefinition>();
        }

        [Required]
        [Key]
        [ForeignKey("User")]
        public string UserId { get; private set; } = null!;

        public string FirstName { get; private set; } = null!;
        
        public string LastName { get; private set; } = null!;

        public DateTime BirthDate { get; private set; }

        public string? Address { get; private set; }

        public Gender Gender { get; private set; }

        public string? AlternativeEmail { get; private set; }


        public int AreaId { get; private set; }

        public int NationalityId { get; private set; }


        public Area? Area { get; private set; }

        public virtual User? User { get; private set; }

        public virtual ICollection<ProfileAccessDefinition> ProfileAccessDefinitions { get; private set; }


        public static UserProfile Create(string userId,
            string firstName,
            string lastName,
            DateTime birthDate,
            string? address,
            Gender gender,
            string? alternativeEmail,
            int areaId,
            int nationalityId)
        {
            return new UserProfile(userId, firstName, lastName, birthDate, address, gender, alternativeEmail, areaId, nationalityId);
        }

        public (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewCoverAccessDefinition) ChangeProfileCover()
        {
            var oldProfileCover = ProfileAccessDefinitions.FirstOrDefault(x => x.Type == ProfileAccessDefinitionType.Cover);

            var newProfileCover = ProfileAccessDefinition.Create(ProfileAccessDefinitionType.Cover);

            ProfileAccessDefinitions.Add(newProfileCover);

            return (oldProfileCover?.AccessDefinitionId, newProfileCover);
        }

        public (int? oldAccessDefinitionId, ProfileAccessDefinition profileNewPictureAccessDefinition) ChangeProfilePicture()
        {
            var oldProfilePicture = ProfileAccessDefinitions.FirstOrDefault(x => x.Type == ProfileAccessDefinitionType.Picture);

            var newProfilePicture = ProfileAccessDefinition.Create(ProfileAccessDefinitionType.Picture);

            ProfileAccessDefinitions.Add(newProfilePicture);

            return (oldProfilePicture?.AccessDefinitionId, newProfilePicture);
        }

        public void Update(string firstName,
            string lastName,
            DateTime birthDate,
            string? address,
            Gender gender,
            string? alternativeEmail,
            int areaId,
            int nationalityId)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Address = address;
            Gender = gender;
            AlternativeEmail = alternativeEmail;
            AreaId = areaId;
            NationalityId = nationalityId;
        }
    }
}
