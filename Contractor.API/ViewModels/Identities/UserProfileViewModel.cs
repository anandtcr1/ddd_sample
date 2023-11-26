using Contractor.Lookups;

namespace Contractor.Identities
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string? Address { get; set; }

        public Gender Gender { get; set; }

        public string? AlternativeEmail { get; set; }


        public int AreaId { get; set; }

        public int NationalityId { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public IFormFile? ProfileCover { get; set; }

        public List<IFormFile>? Attachments { get; set; }
    }
}
