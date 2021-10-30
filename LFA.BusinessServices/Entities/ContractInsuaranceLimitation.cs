using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ContractInsuaranceLimitation
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid BaseProductId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid InsuaranceLimitationId { get; set; }


        
    }
}
