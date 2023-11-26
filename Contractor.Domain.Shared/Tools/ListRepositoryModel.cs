using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tools
{
    public class ListRepositoryModel<T>
    {
        public ListRepositoryModel(int totalCount, List<T> list)
        {
            TotalCount = totalCount;
            List = list;
        }

        public int TotalCount { get; set; }
        public List<T> List { get; set; }
    }
}
