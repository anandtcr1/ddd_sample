using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tools.Memory
{
    public interface IMemoryCacheManager
    {
        void SetSubscriptionValue(string key, long value);

        long GetSubscriptionValue(string key);
    }
}
