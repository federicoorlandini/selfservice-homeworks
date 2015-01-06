using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DataAccess.Caching
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set<T>(T value, string key);

        void Invalidate(string key);
    }
}
