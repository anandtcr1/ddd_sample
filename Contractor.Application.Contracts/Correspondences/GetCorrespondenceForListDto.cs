using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Correspondences
{
    public class GetCorrespondenceForListDto
    {
        public int Id { get; set; }

        public int ThreadId { get; set; }

        public DateTime IssueDate { get; set; }

        public string Subject { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string SenderName { get; set; } = null!;
    }
}
