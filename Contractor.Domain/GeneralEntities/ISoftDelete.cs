using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.GeneralEntities
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
