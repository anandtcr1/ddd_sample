using Contractor.Tools.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tools
{
    public interface IEmailManager
    {
        Task SendEmail(Message message);
    }
}
