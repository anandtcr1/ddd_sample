using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tools
{
    public class AzureUrls
    {
        public BlobStorageUrls BlobStorage { get; set; }
    }

    public class BlobStorageUrls
    {
        public string ContainerName { get; set; } = null!;
        
        public string StorageAccount { get; set; } = null!;
        
        public string Key { get; set; } = null!;
        
        public string ConnectionString { get; set; } = null!;
    }
}
