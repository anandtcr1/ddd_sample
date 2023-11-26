using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class JWTSettings
    {
        public string Secret { get; set; } = null!;
        
        public string ValidIssuer { get; set; } = null!;
        
        public string ValidAudience { get; set; } = null!;
        
        public int Expires { get; set; }
    }
}
