using Amazon.ElastiCacheCluster;
using Enyim.Caching;
using System;

namespace TAS.Caching
{
    internal sealed class AmazonElasticacheClusterCache : Cache
    {

        private MemcachedClient client = null;

        private MemcachedClient Client
        {
            get
            {
                return this.client;
            }
        }

        public AmazonElasticacheClusterCache()
        {
            ElastiCacheClusterConfig config = new ElastiCacheClusterConfig();
            this.client = new MemcachedClient(config);
        }

        protected override object GetValue(string key)
        {
            return this.Client.Get(key);
        }

        public override void SetValue(string key, object value, TimeSpan validFor)
        {
            this.Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value, validFor);
        }

        protected override void RemoveValue(string key)
        {
            this.Client.Remove(key);
        }

    }
}
