using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class RSAProvider
    {
        public virtual Guid Id { get; set; }
        public virtual string ProviderName { get; set; }
        public virtual string ProviderCode { get; set; }
    }
}
