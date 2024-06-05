using Microsoft.Extensions.Caching.Memory;

namespace RGBA.Optio.Core.PerformanceImprovmentServices
{
    public class CacheService(IMemoryCache cache)
    {
        #region GetOrCreate
        public T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan absoluteExpiration)
        {
            if (!cache.TryGetValue(key, out T item))
            {
                item = createItem();

                cache.Set(key, item, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = absoluteExpiration
                });
            }

            return item;
        }
        #endregion

        public void Remove(string key)
        {
            cache.Remove(key);
        }
    }
}
