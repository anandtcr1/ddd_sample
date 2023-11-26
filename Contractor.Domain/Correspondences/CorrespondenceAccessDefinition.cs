using Contractor.Files;

namespace Contractor.Correspondences
{
    public class CorrespondenceAccessDefinition
    {
        private CorrespondenceAccessDefinition() { }


        public int CorrespondenceId { get; private set; }

        public int AccessDefinitionId { get; private set; }

        public virtual Correspondence? Correspondence { get; private set; }

        public virtual AccessDefinition? AccessDefinition { get; private set; }

        public static CorrespondenceAccessDefinition Create()
        {
            return new CorrespondenceAccessDefinition();
        }
    }
}
