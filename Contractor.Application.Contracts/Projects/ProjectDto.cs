using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public int? DraftProjectId { get; set; }

        public int ProjectTypdId { get; set; }

        public string ProjectNumber { get; set; } = null!;

        public string ProjectTitle { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; } = null!;

        public ProjectStatus Status { get; set; }

        public List<ProjectUserDto> ProjectUsers { get; set; }
       
        public List<ProjectInvitationDto> ProjectInvitations { get; set; }

    }
}
