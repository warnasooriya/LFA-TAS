using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class NRPCommissionContractMapping
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid NRPCommissionId { get; set; }
        public virtual Decimal Commission { get; set; }
        public virtual bool IsPercentage { get; set; }
        public virtual bool IsOnNRP { get; set; }
        public virtual bool IsOnGROSS { get; set; }
    }
}
