using Contractor.GeneralViewModels;
using Contractor.Tenders;

namespace Contractor.Tenders
{
    public class GetTenderListViewModel: GetListViewModel
    {
        public string? TenderNumber{ get; set; }
        public DateTime? TenderDate { get; set; }
        public string? TenderTitle { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ProjectNumber { get; set; }
        public TenderStatus? Status { get; set; }
        public string? Winner { get; set; }



    }
}
