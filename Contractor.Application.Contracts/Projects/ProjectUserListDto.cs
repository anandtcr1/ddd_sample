using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectUserListDto
    {
        public int Id { get; set; }
        
        public ProjectUserType UserType { get; set; }
        
        public int ProjectId { get; set; }
        
        public string UserId { get; set; } = null!;
        
        public string? UserDisplayName { get; set; }
    }
}
