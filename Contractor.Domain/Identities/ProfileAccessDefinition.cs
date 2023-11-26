
using Contractor.Files;

namespace Contractor.Identities
{
    public class ProfileAccessDefinition
    {
        private ProfileAccessDefinition(ProfileAccessDefinitionType type)
        {
            Type = type;
        }

        public int Id { get; private set; }

        public ProfileAccessDefinitionType Type { get; private set; }

        public string UserId { get; private set; } = null!;

        public int AccessDefinitionId { get; private set; }

        public virtual UserProfile User { get; private set; }

        public virtual AccessDefinition AccessDefinition { get; private set; }


        public static ProfileAccessDefinition Create(ProfileAccessDefinitionType type)
        {
            return new ProfileAccessDefinition(type);
        }
    }
}
