using Contractor.GeneralViewModels;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Subscriptions
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public float StorageSpace { get; set; }
    }

    public class GetSubScriptionListViewModel : GetListViewModel
    {
        public string? Name { get; set; }
    }
}
