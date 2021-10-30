using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace TAS.Caching
{
    public class AzureRedisCache : Cache
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("contoso5.redis.cache.windows.net,abortConnect=false,ssl=true,password=...");
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public  IDatabase cache { get; set; }
        public AzureRedisCache()
        {
            cache = Connection.GetDatabase();
        }



        protected override object GetValue(string key)
        {
            return JsonConvert.DeserializeObject(cache.StringGet(key));
        }

        public override void SetValue(string key, object value, TimeSpan validFor)
        {
            cache.StringSet(key, JsonConvert.SerializeObject(value));
        }

        protected override void RemoveValue(string key)
        {
            cache.KeyDelete(key);
        }
    }
}
