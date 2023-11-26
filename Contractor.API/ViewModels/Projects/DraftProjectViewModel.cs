using Contractor.GeneralViewModels;
using Contractor.Projects;

namespace Contractor.Projects
{
    public class DraftProjectViewModel
    {
        public int Id { get; set; }
        public string? ConsultantId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GetDraftProjectListViewModel : GetListViewModel
    {
        public string? OwnerName { get; set; }

        public DraftProjectStatus? statusId { get; set; }

        public DateTime? createdDate { get; set; }
    }
}
