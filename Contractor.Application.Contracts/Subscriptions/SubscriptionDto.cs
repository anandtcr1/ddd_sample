using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Subscriptions
{
    public class SubscriptionDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public float StorageSpace { get; set; }

        public List<ProjectFolderTemplateDto>? ProjectFolderTemplates { get; set; }
    }
}
