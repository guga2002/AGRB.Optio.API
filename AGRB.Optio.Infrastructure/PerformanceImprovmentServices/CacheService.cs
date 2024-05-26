using Microsoft.Extensions.Caching.Memory;

namespace RGBA.Optio.Core.PerformanceImprovmentServices
{
    public class CacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        #region GetOrCreate
        public T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan absoluteExpiration)
        {
            if (!_cache.TryGetValue(key, out T item))
            {
                item = createItem();

                _cache.Set(key, item, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = absoluteExpiration
                });
            }

            return item;
        }
        #endregion

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
