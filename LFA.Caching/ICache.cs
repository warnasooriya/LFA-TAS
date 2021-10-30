using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Caching
{
    public interface ICache
    {

        object this[string key]
        {
            get;
            set;
        }

        void Remove(string key);

    }
}
