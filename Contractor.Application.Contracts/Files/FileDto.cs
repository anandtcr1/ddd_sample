using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class FileDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public float Size { get; set; }

        public string Token { get; set; } = null!;

        public string Path { get; set; } = null!;

        public string Extension { get; set; } = null!;

        public FileType FileType { get; set; }
    }
}
