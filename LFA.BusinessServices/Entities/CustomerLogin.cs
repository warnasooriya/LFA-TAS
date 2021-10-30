using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CustomerLogin
    {
        public virtual Guid Id { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerUserID { get; set; }
        public virtual string JwtToken { get; set; }
        public virtual DateTime IssuedDateTime { get; set; }
        public virtual DateTime LastRequestDateTime { get; set; }
        public virtual Guid IssuedCountryRegion { get; set; }
        public virtual bool IsExpired { get; set; }
    }
}
