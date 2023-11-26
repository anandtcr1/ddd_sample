using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class DraftProjectDto
    {
        public int Id { get; private set; }
        public string? OwnerId { get; set; }
        public string? ConsultantId { get; set; }
        public DraftProjectStatus StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
