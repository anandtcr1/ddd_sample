using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class AccessDefinitionDto
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? OriginalId { get; set; }

        public string Path { get; set; } = null!;

        public bool IsOriginal { get; set; }

        public string UserId { get; set; } = null!;

        public AccessDefinitionStatus Status { get; set; }

        public AccessDefinitionType Type { get; set; }

        public bool Deletable { get; set; }

        public FileDto? File { get; set; }
    }
}
