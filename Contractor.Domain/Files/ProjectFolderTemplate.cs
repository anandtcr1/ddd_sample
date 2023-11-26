using Contractor.Subscriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class ProjectFolderTemplate
    {
        private ProjectFolderTemplate(string name, int subscriptionId)
        {
            Name = name;
            SubscriptionId = subscriptionId;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; } = null!;

        [ForeignKey("Subscription")]
        public int SubscriptionId { get; private set; }

        public virtual Subscription? Subscription { get; private set; }


        public static ProjectFolderTemplate Create(string name, int subscriptionId)
        {
            return new ProjectFolderTemplate(name, subscriptionId);
        }
    }
}
