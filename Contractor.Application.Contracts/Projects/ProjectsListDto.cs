using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectsListDto
    {
        public int Id { get; set; }

        public string ProjectNumber { get; set; } = null!;

        public string ProjectTypeEnglish { get; set; } = null!;

        public string ProjectTypeArabic { get; set; } = null!;

        public string? ProjectOwner { get; set; }

        public string ProjectTitle { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ProjectStatus Status { get; set; }
    }
}
