using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Caching
{
    public abstract class Cache : ICache
    {

        public object this[string key]
        {
            get
            {
                return this.GetValue(key);
            }
            set
            {
                this.SetValue(key, value);
            }
        }

        public void Remove(string key)
        {
            this.RemoveValue(key);
        }

        protected abstract object GetValue(string key);

        protected virtual void SetValue(string key, object value)
        {
            this.SetValue(key, value, TimeSpan.MaxValue);
        }

        public virtual void SetValue(string key, object value, DateTime expiresAt)
        {
            TimeSpan diff = expiresAt.Subtract(DateTime.Now);

            this.SetValue(key, value, diff);
        }
        
        public abstract void SetValue(string key, object value, TimeSpan validFor);

        protected abstract void RemoveValue(string key);

    }
}
