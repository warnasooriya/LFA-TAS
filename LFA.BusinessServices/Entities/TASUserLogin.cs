using System;

namespace TAS.Services.Entities
{
    public class TASUserLogin
    {
        public virtual Guid ID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string SystemUserID { get; set; }
        public virtual string JwtToken { get; set; }
        public virtual DateTime IssuedDateTime { get; set; }
        public virtual DateTime LastRequestDateTime { get; set; }
        public virtual Guid IssuedCountryRegion { get; set; }
        public virtual bool IsExpired { get; set; }

    }
}