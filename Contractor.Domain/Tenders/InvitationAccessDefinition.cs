using Contractor.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public class InvitationAccessDefinition
    {
        private InvitationAccessDefinition() { }

        public int TenderInvitationId { get; private set; }

        public int AccessDefinitionId { get; private set; }

        public virtual TenderInvitation? TenderInvitation { get; private set; }

        public virtual AccessDefinition? AccessDefinition { get; private set; }


        public static InvitationAccessDefinition Create()
        {
            return new InvitationAccessDefinition();
        }
    }
}
