using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class ForgotPasswordResponse
    {
        public string Token { get; set; } = null!;

        public string RedirectUrl { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
