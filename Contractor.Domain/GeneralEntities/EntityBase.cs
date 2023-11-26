using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.GeneralEntities
{
    public abstract class EntityBase<Tkey>
    {
        public Tkey Id { get; set; } = default!;
    }
}
