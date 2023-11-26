using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.GeneralEntities
{
    public interface IAuditEntity
    {
        public DateTime InsertDate { get; set; }

        public string InsertUserId { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string? LastModifyUserId { get; set; }

        public DateTime? DeleteDate { get; set; }

        public string? DeleteUserId { get; set; }
    }
}
