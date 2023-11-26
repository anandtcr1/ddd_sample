using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class TokenResponse
    {
        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }
    }
}
