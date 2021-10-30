using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ReinsurerConsortium
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ParentReinsurerId { get; set; }
        public virtual Guid ReinsurerId { get; set; }
        public virtual Decimal NRPPercentage { get; set; }
        public virtual Decimal RiskSharePercentage { get; set; }
        public virtual Decimal ProfitSharePercentage { get; set; }
    }
}
