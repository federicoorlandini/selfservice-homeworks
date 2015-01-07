using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DataAccess.Caching
{
    public interface ICacheProvider
    {
        T Get<T>(string key) where T : class;
        void Set(object value, string key);

        void Invalidate(string key);
    }
}
