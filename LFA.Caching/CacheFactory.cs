using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace TAS.Caching
{
    public static class CacheFactory
    {

        public static ICache GetCache()
        {
#if DEBUG
            return new EnyimCache();
        #else

                    return new AmazonElasticacheClusterCache();
        #endif

            // return new EnyimCache();
        }

    }
}
