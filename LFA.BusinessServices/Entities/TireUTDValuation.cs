using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TireUTDValuation
    {
        public virtual Guid Id { get; set; }
        public virtual Decimal MinUTD { get; set; }
        public virtual Decimal MaxUTD { get; set; }
        public virtual Decimal ClaimPercentage { get; set; }
    }
}
