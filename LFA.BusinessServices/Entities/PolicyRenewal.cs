using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PolicyRenewal
    {
        public virtual Guid Id { get; set; }
        public virtual Guid OriginalPolicyBundleId { get; set; }
        public virtual Guid NewPolicyBundleId { get; set; }
        public virtual DateTime RenewedDate { get; set; }
        public virtual Guid RenewedBy { get; set; }
        public virtual decimal   RenewalFee { get; set; }
        public virtual Guid PolicyTransactionId { get; set; }
        public virtual Guid PolicyBundleHistoryId { get; set; }

    }
}
