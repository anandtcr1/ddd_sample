using Contractor.GeneralEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public sealed class Company:EntityBase<int>
    {
        private Company()
        {
            Users = new HashSet<User>();
        }

        public ICollection<User> Users { get; private set; }

        public static Company Create()
        {
            return new Company();
        }

        public void Add(User user)
        {
            Users.Add(user);
        }
         
    }
}
