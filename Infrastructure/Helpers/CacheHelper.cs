using Application.Helpers;
using Common.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IMemoryCache memoryCache;
        private readonly AppSettings appSettings;

        public CacheHelper(IMemoryCache memoryCache, IOptions<AppSettings> options)
        {
            this.memoryCache = memoryCache;
            appSettings = options.Value;
        }

        public void Set<T>(string key, T value)
        {
            Remove(key);
            var entryOptions = new MemoryCacheEntryOptions();
            entryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(appSettings.Cache.TimeoutInMinutes));
            memoryCache.Set<T>(key, value, entryOptions);
        }

        public T Get<T>(string key)
        {
            memoryCache.TryGetValue<T>(key, out var value);
            return value;
        }

        public void Remove(string key)
        {
            if (memoryCache.TryGetValue(key, out var value))
            {
                memoryCache.Remove(key);
            }
        }
    }
}
