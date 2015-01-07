using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DataAccess.Caching
{
    /// <summary>
    /// This is the interface to handle the cache.
    /// In a real application, we can use different caching system like Redis.
    /// In this case, I used a single in-memory cache system.
    /// We can improve this implementation adding an expiring time for every cache entry.
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private IDictionary<string, object> _cache = new Dictionary<string, object>();

        public T Get<T>(string key) where T : class
        {
            if( _cache.ContainsKey(key) )
            {
                return _cache[key] as T;
            }
            return null;
        }

        public void Set(object value, string key)
        {
            _cache[key] = value;
        }

        public void Invalidate(string key)
        {
            _cache.Remove(key);
        }
    }
}
