using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class RSAAnualPremium
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual int Year { get; set; }
        public virtual Decimal Value { get; set; }
    }
}
