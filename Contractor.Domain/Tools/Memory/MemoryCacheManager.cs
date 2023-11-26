using Microsoft.Extensions.Caching.Memory;

namespace Contractor.Tools.Memory
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SetSubscriptionValue(string key, long value)
        {
            _memoryCache.Set(key, value);
        }

        public long GetSubscriptionValue(string key)
        {
            if(_memoryCache.TryGetValue(key, out long value))
                return value;

            return -1;

        }
    }
}
