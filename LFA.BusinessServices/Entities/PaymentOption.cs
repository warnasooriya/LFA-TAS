using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PaymentOptions
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PaymentModeId { get; set; }
        public virtual string PaymentOption { get; set; }
        public virtual decimal PaymentCharge { get; set; }
    }
}
