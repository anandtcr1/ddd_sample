using Contractor.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class TenderAccessDefinition
    {
        private TenderAccessDefinition() { }


        public int TenderId { get; private set; }

        public int AccessDefinitionId { get; private set; }

        public virtual Tender? Tender { get; private set; }

        public virtual AccessDefinition? AccessDefinition { get; private set; }


        public static TenderAccessDefinition Create()
        {
            return new TenderAccessDefinition();
        }
    }
}
