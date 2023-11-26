using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class DraftProjectListDto
    {
        public int Id { get; set; }
        public DraftProjectStatus StatusId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string ProjectOwner { get; set; }
        public string OwnerId { get; set; }
    }
}
