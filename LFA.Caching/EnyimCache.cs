using Enyim.Caching;
using System;

namespace TAS.Caching
{
    internal sealed class EnyimCache : Cache
    {
        
        private MemcachedClient client = null;

        private MemcachedClient Client
        {
            get
            {
                return this.client;
            }
        }

        public EnyimCache()
        {
            this.client = new MemcachedClient();
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
