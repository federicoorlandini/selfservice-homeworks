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
    /// In this case, I used a single in-memory cache system
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(T value, string key)
        {
            throw new NotImplementedException();
        }

        public void Invalidate(string key)
        {
            throw new NotImplementedException();
        }
    }
}
