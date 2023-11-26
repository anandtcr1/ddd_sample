using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class FileDownloadResponse
    {
        public Stream Content { get; set; } = null!;

        public string ContentType { get; set; } = null!;
    }
}
