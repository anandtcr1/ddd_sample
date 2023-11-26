using Contractor.GeneralViewModels;
using Contractor.Tenders;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Tenders
{
    public class TenderInvitationViewModel 
    {
        public int TenderId { get; set; }

        [Required]
        public List<string> UserIdList { get; set; } = null!;
         

    }
}
