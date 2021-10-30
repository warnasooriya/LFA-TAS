using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class InsurerConsortium
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ParentInsurerId { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Decimal NRPPercentage { get; set; }
        public virtual Decimal RiskSharePercentage { get; set; }
        public virtual Decimal ProfitSharePercentage { get; set; }
    }
}
