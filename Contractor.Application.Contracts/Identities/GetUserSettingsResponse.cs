using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class GetUserSettingsResponse
    {
        public string UserId { get; set; } = null!;

        public UserTypes UserType { get; set; }

        public List<PageDto> Pages { get; set; }

        public List<string> Functions { get; set; }

        public long DataUsage { get; set; }

        public float StorageSpace { get; set; }
    }
}
