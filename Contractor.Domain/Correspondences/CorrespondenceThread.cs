using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Correspondences
{
    public class CorrespondenceThread
    {
        private CorrespondenceThread() 
        {
            Correspondences = new HashSet<Correspondence>();
        }

        [Key]
        public int Id { get; private set; }

        public virtual ICollection<Correspondence> Correspondences { get; private set; }


        public static CorrespondenceThread Create()
        {
            return new CorrespondenceThread();
        }

        public void AddCorrespondence(Correspondence correspondence)
        {
            Correspondences.Add(correspondence);
        }
    }
}
